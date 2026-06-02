using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class UpgradeRankSOData
{
    public int numberTier;
    public int cardRequire;
    public string content;
    public float value;
    public float time;
    public float cooldown;
}

[Serializable]
public class UpgradeSOData
{
    public string upgrade;
    public int attack;
    public int defend;
    public int energy;
    public float speed;
    public int gold;
    public int crystal;
}

[CreateAssetMenu(fileName = "PlayerPlaneConfig", menuName = "Config/Player Plane")]
public class PlayerPlaneConfigSO : ScriptableObject
{
    [Header("Visuals")]
    public Sprite planeSprite;
    public Sprite supportPlaneSprite;

    public bool isReaded;
    public float cost;
    public string detail;
    public string namePlane;
    public List<UpgradeRankSOData> rank;
    public List<UpgradeSOData> upgrade;
}
