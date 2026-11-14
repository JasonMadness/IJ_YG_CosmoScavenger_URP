using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCatalog", menuName = "Progress/UpgradeCatalog")]
public class UpgradeCatalog : ScriptableObject
{
    [SerializeField] private List<PlayerUpgrade> _upgrades = new();

    public IReadOnlyList<PlayerUpgrade> Upgrades => _upgrades;
}
