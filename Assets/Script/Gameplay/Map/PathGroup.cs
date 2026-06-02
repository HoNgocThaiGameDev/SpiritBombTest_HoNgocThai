using UnityEngine;
using System.Collections;

public class PathGroup : MonoBehaviour {

	// Use this for initialization
    public static PathGroup instance;
    public GameObject[] pathList;
    public Vector3[] posPathList;
	void Awake () {
        Initialize();
	}

    void OnEnable()
    {
        Initialize();
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        instance = this;
        if (pathList == null)
        {
            return;
        }

        if (posPathList == null || posPathList.Length < pathList.Length)
        {
            posPathList = new Vector3[50];
        }

        for (int i = 0; i < pathList.Length; i++)
        {
            EnemyPath path = pathList[i] != null ? pathList[i].GetComponent<EnemyPath>() : null;
            if (path != null && path.nodes != null && path.nodes.Count > 0)
            {
                posPathList[i] = path.nodes[0];
            }
        }
    }
	
}
