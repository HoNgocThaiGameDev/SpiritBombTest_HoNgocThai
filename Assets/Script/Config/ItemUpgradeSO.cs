using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class MissileSOData
{
    public string upgrade;
    public int cost;
    public int attack;
    public int speed;
    public int number;
}

[Serializable]
public class SupportSOData
{
    public string upgrade;
    public int cost;
    public int attack;
    public float speed;
    public int duration;
}

[Serializable]
public class ShieldSOData
{
    public string upgrade;
    public int cost;
    public int defend;
    public int duration;
}

[CreateAssetMenu(fileName = "ItemUpgradeConfig", menuName = "Config/ItemUpgradeConfig")]
public class ItemUpgradeSO : ScriptableObject
{
    [Header("Missile Upgrades")]
    public bool missileIsReaded;
    public List<MissileSOData> missileUpgrades;

    [Header("Support Upgrades")]
    public bool supportIsReaded;
    public List<SupportSOData> supportUpgrades;

    [Header("Shield Upgrades")]
    public bool shieldIsReaded;
    public List<ShieldSOData> shieldUpgrades;
}
