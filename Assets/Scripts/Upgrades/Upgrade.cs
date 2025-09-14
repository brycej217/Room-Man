using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public new string name;

    public abstract void ApplyUpgrade(Player player);
}
