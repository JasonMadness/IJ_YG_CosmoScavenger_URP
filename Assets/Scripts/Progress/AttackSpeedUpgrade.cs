using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Progress/Upgrades/AttackSpeed")]
public class AttackSpeedUpgrade : PlayerUpgrade
{
    [SerializeField] private float _attackSpeedPercent = 0.2f;

    public override void Apply(Player player)
    {
        player.IncreaseAttackSpeed(_attackSpeedPercent);
    }
}
