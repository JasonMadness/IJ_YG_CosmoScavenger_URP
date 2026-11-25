param(
    [string]$Method = '',
    [string]$Params = '',
    [switch]$Fresh,
    [switch]$List
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

$baseUrl = 'http://127.0.0.1:8080/mcp'
$sessionFile = Join-Path $PSScriptRoot 'session.txt'

function Send-McpRequest {
    param([object]$BodyObject, [string]$SessionId)

    $json = $BodyObject | ConvertTo-Json -Depth 100 -Compress
    $headers = @{ 'Accept' = 'application/json, text/event-stream' }
    if ($SessionId) { $headers['Mcp-Session-Id'] = $SessionId }

    $bodyBytes = [System.Text.Encoding]::UTF8.GetBytes($json)

    return Invoke-WebRequest -Uri $baseUrl -Method Post -Body $bodyBytes `
        -ContentType 'application/json; charset=utf-8' -Headers $headers `
        -UseBasicParsing -TimeoutSec 120
}

function Get-SseData {
    param([string]$Content)

    $lines = $Content -split "`n"
    $dataLines = @($lines | Where-Object { $_.StartsWith('data:') } | ForEach-Object { $_.Substring(5).Trim() })
    if ($dataLines.Count -gt 0) {
        return ($dataLines -join "`n")
    }
    return $Content
}

$sessionId = $null
if (-not $Fresh -and (Test-Path $sessionFile)) {
    $sessionId = (Get-Content $sessionFile -Raw).Trim()
}

if ($Fresh -or -not $sessionId) {
    $initBody = @{
        jsonrpc = '2.0'
        id      = 1
        method  = 'initialize'
        params  = @{
            protocolVersion = '2025-03-26'
            capabilities    = @{}
            clientInfo      = @{ name = 'dsh-unity-helper'; version = '1.0' }
        }
    }
    $initResponse = Send-McpRequest -BodyObject $initBody -SessionId $null
    $sessionId = $initResponse.Headers['mcp-session-id']
    if (-not $sessionId) { throw 'No mcp-session-id returned' }
    Set-Content -Path $sessionFile -Value $sessionId -NoNewline

    $initialized = @{
        jsonrpc = '2.0'
        method  = 'notifications/initialized'
    }
    try {
        Send-McpRequest -BodyObject $initialized -SessionId $sessionId | Out-Null
    } catch {
        Write-Host "initialized notification: $($_.Exception.Message)"
    }
}

if ($List) {
    $Method = 'tools/list'
    $Params = ''
}

$id = Get-Random -Minimum 2 -Maximum 1000000
$requestBody = @{
    jsonrpc = '2.0'
    id      = $id
    method  = $Method
}
if ($Params -and $Params.Trim() -ne '') {
    $requestBody['params'] = $Params | ConvertFrom-Json
}

try {
    $response = Send-McpRequest -BodyObject $requestBody -SessionId $sessionId
} catch {
    $status = $null
    if ($_.Exception.Response) { $status = [int]$_.Exception.Response.StatusCode }
    if ($status -eq 404 -or $status -eq 400) {
        Remove-Item $sessionFile -ErrorAction SilentlyContinue
        & $PSCommandPath -Method $Method -Params $Params -Fresh
        exit $LASTEXITCODE
    }
    throw
}

$data = Get-SseData -Content $response.Content
try {
    $parsed = $data | ConvertFrom-Json
    $parsed | ConvertTo-Json -Depth 100
} catch {
    $data
}
