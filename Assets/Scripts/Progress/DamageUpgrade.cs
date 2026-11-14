using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Progress/Upgrades/Damage")]
public class DamageUpgrade : PlayerUpgrade
{
    [SerializeField] private float _damagePercent = 0.25f;

    public override void Apply(Player player)
    {
        player.IncreaseDamage(_damagePercent);
    }
}
