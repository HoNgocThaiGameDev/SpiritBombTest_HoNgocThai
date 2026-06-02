using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCatalog", menuName = "Config/Enemy Catalog")]
public class EnemyCatalogSO : ScriptableObject
{
    public bool isReaded = true;
    public List<EnemyConfigData> enemies = new List<EnemyConfigData>();
}
