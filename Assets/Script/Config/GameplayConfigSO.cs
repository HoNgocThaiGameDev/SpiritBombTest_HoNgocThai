using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyType
{
    Basic = 1,
    Heavy = 2,
    Boss = 21
}

public enum EnemyMovementMode
{
    StraightDown = 0,
    FollowPath = 1,
    FormationHold = 2,
    FormationSine = 3,
    FormationCircle = 4,
    FormationDiagonalHold = 5
}

[Serializable]
public class EnemyConfigData
{
    public EnemyType enemyType;
    public string type;
    public Sprite sprite;
    public int health;
    public int score;
    public int attack;
    public int defend;
    public float fireRate;
}

[Serializable]
public class EnemyWaveConfigData
{
    public EnemyType enemyType;
    [FormerlySerializedAs("rowPos")]
    public int rowPosition;
    [FormerlySerializedAs("columPos")]
    public int columnPosition;
    public int itemDrop;
}

[Serializable]
public class WaveConfigData
{
    public int waveNumber;
    [FormerlySerializedAs("waveDelay")]
    public float nextWaveDelay;
    [FormerlySerializedAs("enemyDelay")]
    public float spawnInterval;
    public EnemyMovementMode movementMode;
    [FormerlySerializedAs("path")]
    public int pathId;
    public float verticalSpeed;
    [FormerlySerializedAs("enemySpeed")]
    public float pathTravelDuration;
    public List<EnemyWaveConfigData> enemyList = new List<EnemyWaveConfigData>();
}

[Serializable]
public class PathConfigData
{
    public int pathId;
    [FormerlySerializedAs("pathNumber")]
    public int laneCount;
}
