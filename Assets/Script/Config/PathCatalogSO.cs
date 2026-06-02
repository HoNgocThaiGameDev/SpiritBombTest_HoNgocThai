using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathCatalog", menuName = "Config/Path Catalog")]
public class PathCatalogSO : ScriptableObject
{
    public bool isReaded = true;
    public List<PathConfigData> paths = new List<PathConfigData>();
}
