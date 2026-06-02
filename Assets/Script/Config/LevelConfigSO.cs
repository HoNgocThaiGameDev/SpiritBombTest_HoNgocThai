using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    public int levelNumber;
    public bool isReaded = true;
    public int energy;
    public List<WaveConfigData> waves = new List<WaveConfigData>();
}
