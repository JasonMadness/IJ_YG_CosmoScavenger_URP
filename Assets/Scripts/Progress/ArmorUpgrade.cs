using UnityEngine;

[CreateAssetMenu(fileName = "ArmorUpgrade", menuName = "Progress/Upgrades/Armor")]
public class ArmorUpgrade : PlayerUpgrade
{
    [SerializeField] private float _armorAmount = 0.1f;

    public override void Apply(Player player)
    {
        player.IncreaseArmor(_armorAmount);
    }
}
