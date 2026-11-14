using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "Progress/Upgrades/MoveSpeed")]
public class MoveSpeedUpgrade : PlayerUpgrade
{
    [SerializeField] private float _speedPercent = 0.15f;

    public override void Apply(Player player)
    {
        player.IncreaseMoveSpeed(_speedPercent);
    }
}
