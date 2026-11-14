using UnityEngine;

public abstract class PlayerUpgrade : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;

    public string Title => _title;

    public string Description => _description;

    public abstract void Apply(Player player);
}
