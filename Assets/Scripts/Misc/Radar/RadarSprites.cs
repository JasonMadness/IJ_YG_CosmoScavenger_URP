using UnityEngine;

public static class RadarSprites
{
    private const int ArrowResolution = 64;
    private const int CircleResolution = 128;
    private const int SamplesPerAxis = 4;
    private const float CircleRadius = 0.5f;
    private const float RimStart = 0.4f;
    private const float FillAlpha = 0.35f;
    private const float RimAlpha = 0.9f;

    private static Sprite _arrow;
    private static Sprite _circle;

    public static Sprite Arrow
    {
        get
        {
            if (_arrow == null)
                _arrow = CreateArrow();

            return _arrow;
        }
    }

    public static Sprite Circle
    {
        get
        {
            if (_circle == null)
                _circle = CreateCircle();

            return _circle;
        }
    }

    private static Sprite CreateArrow()
    {
        Texture2D texture = CreateTexture(ArrowResolution, "RadarArrow");

        Vector2 apex = new Vector2(0.5f, 0.97f);
        Vector2 left = new Vector2(0.12f, 0.03f);
        Vector2 right = new Vector2(0.88f, 0.03f);

        Color[] pixels = new Color[ArrowResolution * ArrowResolution];

        for (int y = 0; y < ArrowResolution; y++)
        {
            for (int x = 0; x < ArrowResolution; x++)
            {
                float coverage = GetTriangleCoverage(x, y, apex, left, right);
                pixels[y * ArrowResolution + x] = new Color(1f, 1f, 1f, coverage);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return CreateSprite(texture, ArrowResolution);
    }

    private static Sprite CreateCircle()
    {
        Texture2D texture = CreateTexture(CircleResolution, "RadarCircle");

        Color[] pixels = new Color[CircleResolution * CircleResolution];
        float feather = 1.5f / CircleResolution;

        for (int y = 0; y < CircleResolution; y++)
        {
            for (int x = 0; x < CircleResolution; x++)
            {
                float pixelX = (x + 0.5f) / CircleResolution - 0.5f;
                float pixelY = (y + 0.5f) / CircleResolution - 0.5f;
                float distance = Mathf.Sqrt(pixelX * pixelX + pixelY * pixelY);

                float outer = Mathf.InverseLerp(CircleRadius, CircleRadius - feather, distance);
                float rim = Mathf.InverseLerp(RimStart - feather, RimStart + feather, distance);

                pixels[y * CircleResolution + x] = new Color(1f, 1f, 1f, Mathf.Lerp(FillAlpha, RimAlpha, rim) * outer);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return CreateSprite(texture, CircleResolution);
    }

    private static Texture2D CreateTexture(int resolution, string textureName)
    {
        return new Texture2D(resolution, resolution, TextureFormat.RGBA32, false)
        {
            name = textureName,
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
            hideFlags = HideFlags.HideAndDontSave
        };
    }

    private static Sprite CreateSprite(Texture2D texture, int resolution)
    {
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0f, 0f, resolution, resolution),
            new Vector2(0.5f, 0.5f),
            resolution,
            0,
            SpriteMeshType.FullRect);

        sprite.name = texture.name;

        return sprite;
    }

    private static float GetTriangleCoverage(int x, int y, Vector2 a, Vector2 b, Vector2 c)
    {
        float hits = 0f;
        float step = 1f / SamplesPerAxis;

        for (int sampleY = 0; sampleY < SamplesPerAxis; sampleY++)
        {
            for (int sampleX = 0; sampleX < SamplesPerAxis; sampleX++)
            {
                Vector2 point = new Vector2(
                    (x + (sampleX + 0.5f) * step) / ArrowResolution,
                    (y + (sampleY + 0.5f) * step) / ArrowResolution);

                if (IsInsideTriangle(point, a, b, c))
                    hits += 1f;
            }
        }

        return hits / (SamplesPerAxis * SamplesPerAxis);
    }

    private static bool IsInsideTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        float first = Cross(point - a, b - a);
        float second = Cross(point - b, c - b);
        float third = Cross(point - c, a - c);

        bool hasNegative = first < 0f || second < 0f || third < 0f;
        bool hasPositive = first > 0f || second > 0f || third > 0f;

        return !(hasNegative && hasPositive);
    }

    private static float Cross(Vector2 first, Vector2 second)
    {
        return first.x * second.y - first.y * second.x;
    }
}
