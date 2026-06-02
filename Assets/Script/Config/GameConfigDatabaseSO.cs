using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameConfigDatabase", menuName = "Config/Game Config Database")]
public class GameConfigDatabaseSO : ScriptableObject
{
    public PlayerPlaneConfigSO playerPlane;
    [FormerlySerializedAs("itemUpgrades")]
    public ItemUpgradeSO powerUpUpgrades;
    public EnemyCatalogSO enemyCatalog;
    [FormerlySerializedAs("pathData2")]
    public PathCatalogSO formationPaths;
    public List<LevelConfigSO> levels = new List<LevelConfigSO>();

    public LevelConfigSO GetLevel(int levelNumber)
    {
        if (levels == null)
            return null;

        for (int i = 0; i < levels.Count; i++)
        {
            LevelConfigSO level = levels[i];
            if (level != null && level.levelNumber == levelNumber)
                return level;
        }

        int index = levelNumber - 1;
        if (index >= 0 && index < levels.Count)
            return levels[index];

        return null;
    }
}
