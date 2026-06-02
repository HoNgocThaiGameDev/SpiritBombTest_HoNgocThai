using UnityEngine;
using System.Collections;

public class GameplayMapController : MonoBehaviour
{
    public static GameplayMapController instance;

    public GameObject[] map;
    public GameObject rainEffect;
    public GameObject waveManager;
    public GameObject pathList;
    public int level;
    public float mapScrollSpeed = 0.84f;
    private Transform activeMap;
    private Vector3 activeMapStartPosition;
    private float activeMapLoopHeight;
    void Awake()
    {
        instance = this;
        level = GameState.currentLevel.Value;
        GameConfigService.Instance.GetLevel(level);
        if (map != null && map.Length > 0 && map[0] != null)
        {
            map[0].SetActive(true);
        }

        if (level >= 2 && rainEffect != null)
        {
            int random = Random.Range(0, 2);
            if (random == 1)
            {
                rainEffect.SetActive(true);
            }
        }

        CacheActiveMap();
    }

    void Start()
    {
        GameState.isFire = false;
        waveManager.SetActive(true);
        pathList.SetActive(true);
    }

    void Update()
    {
        if (!GameState.isGamePaused && GameState.isMoveMap)
        {
            ScrollActiveMap();
        }
    }

    private void CacheActiveMap()
    {
        for (int i = 0; i < map.Length; i++)
        {
            GameObject mapObject = map[i];
            if (mapObject != null && mapObject.activeSelf)
            {
                activeMap = mapObject.transform;
                activeMapStartPosition = activeMap.position;
                Renderer renderer = mapObject.GetComponentInChildren<Renderer>();
                activeMapLoopHeight = renderer != null ? renderer.bounds.size.y : 0f;
                return;
            }
        }
    }

    private void ScrollActiveMap()
    {
        if (activeMap == null)
        {
            return;
        }

        activeMap.position += Vector3.down * mapScrollSpeed * Time.deltaTime;
        if (activeMapLoopHeight > 0f && activeMap.position.y <= activeMapStartPosition.y - activeMapLoopHeight)
        {
            activeMap.position += Vector3.up * activeMapLoopHeight;
        }
    }
}
