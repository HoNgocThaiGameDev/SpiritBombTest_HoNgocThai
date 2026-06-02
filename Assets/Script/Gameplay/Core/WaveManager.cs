using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public GameObject[] enemyList;
    public GameObject[] boss;
    public int countEnemy;

    public int numberWave = 0;
    private List<WaveConfigData> waves;
    public float timeNextWave = 0;
    private float timeEnemyDelay = 0;
    private int numberEnemy = 0;
    public int enemyPath = 0;

    private float timeNext;
    public bool isShowWin;
    private int indexPathList;
    private WaitForSeconds waitWin;
    private int loadedLevel = -1;
    // Use this for initialization
    void Start()
    {
        instance = this;
        timeNext = 0;
        countEnemy = 0;
        isShowWin = false;
        timeNextWave = 5;
        waitWin = new WaitForSeconds(1f);
        EnsureLevelData();
    }

    void Update()
    {
        if (GameState.isGamePaused == false && EnsureLevelData())
        {
            timeNext += Time.deltaTime;
            if (timeNext >= timeNextWave && numberWave < waves.Count && CanStartNextWave())
            {
                CreateEnemy();
                numberWave++;
                timeNext = 0;
            }
            else if (numberWave >= waves.Count && countEnemy <= 0 && CanShowWin())
            {
                if (!isShowWin)
                {
                    StartCoroutine(WaitWin());
                    isShowWin = true;
                }
            }
        }
    }

    private bool CanStartNextWave()
    {
        return numberWave == 0 || countEnemy <= 0;
    }

    private bool EnsureLevelData()
    {
        if (loadedLevel == GameState.currentLevel.Value && waves != null)
        {
            return true;
        }

        LevelConfigSO level = GameConfigService.Instance.GetLevel(GameState.currentLevel.Value);
        waves = level != null ? level.waves : null;
        if (waves == null)
        {
            Debug.LogError("[WaveManager] Level data is not ready for level " + GameState.currentLevel.Value);
            return false;
        }

        loadedLevel = GameState.currentLevel.Value;
        GameState.currentTotalEnemy.Value = GetTotalEnemyCount(waves);
        return true;
    }

    private static int GetTotalEnemyCount(List<WaveConfigData> levelWaves)
    {
        int totalEnemy = 0;
        for (int i = 0; i < levelWaves.Count; i++)
        {
            WaveConfigData wave = levelWaves[i];
            if (wave != null && wave.enemyList != null)
            {
                totalEnemy += wave.enemyList.Count;
            }
        }

        return totalEnemy;
    }

    IEnumerator WaitWin()
    {
        yield return waitWin;
        if (GamePlayEventListener.instance == null)
        {
            yield break;
        }

        if (GamePlayEventListener.instance.RightCanvas != null)
        {
            GamePlayEventListener.instance.RightCanvas.SetActive(false);
        }
        yield return waitWin;
        GameState.won = true;
    }

    private bool CanShowWin()
    {
        return GamePlayEventListener.instance != null
            && (GamePlayEventListener.instance.saveMePanel == null
                || !GamePlayEventListener.instance.saveMePanel.activeInHierarchy);
    }

private void CreateEnemy()
    {
        WaveConfigData currentWave = waves[numberWave];
        timeEnemyDelay = 0;
        enemyPath = currentWave.pathId;
        numberEnemy = currentWave.enemyList.Count;
        CheckIndexPathList();

        int laneCount = 1;
        if (currentWave.movementMode == EnemyMovementMode.FollowPath)
        {
            laneCount = Mathf.Max(1, GameConfigService.Instance.GetFormationPath(enemyPath).laneCount);
        }

        for (int i = 0; i < numberEnemy; i++)
        {
            int laneIndex = currentWave.movementMode == EnemyMovementMode.StraightDown || laneCount == 1 ? 0 : (i % laneCount) + 1;
            InstanceEnemy(laneIndex, i);
        }

        timeNextWave = currentWave.nextWaveDelay;
    }

private void InstanceEnemy(int indexPath, int index)
    {
        WaveConfigData currentWave = waves[numberWave];
        if (index >= currentWave.enemyList.Count)
        {
            return;
        }

        EnemyWaveConfigData enemyWave = currentWave.enemyList[index];
        if (enemyWave.enemyType == EnemyType.Boss)
        {
            if (boss != null && boss.Length > 0 && boss[0] != null)
            {
                boss[0].SetActive(true);
            }
            else
            {
                Debug.LogError("[WaveManager] Boss1 is not assigned.");
            }
            return;
        }

        if (enemyWave.enemyType != EnemyType.Basic && enemyWave.enemyType != EnemyType.Heavy)
        {
            Debug.LogWarning("[WaveManager] Unsupported enemy type skipped: " + enemyWave.enemyType);
            return;
        }

        EnemyControl enemy = GameManager.Instance.enemyPool.New();
        if (enemy == null)
        {
            return;
        }

        enemy.SetInfo(enemyWave.enemyType, GetEnemyHealthScale());
        enemy.ConfigureCombat(GetEnemyAttackScale(), GetEnemyFireRateMultiplier(), CanEnemyShoot(currentWave, enemyWave));
        enemy.idItemDrop = enemyWave.itemDrop;
        enemy.transform.rotation = enemyList[0].transform.rotation;
        if (currentWave.movementMode == EnemyMovementMode.StraightDown)
        {
            enemy.movementMode = EnemyMovementMode.StraightDown;
            enemy.speed = currentWave.verticalSpeed;
            float xPos = ClampToVisibleX(enemyWave.columnPosition * 0.8f - 2.8f);
            float yPos = enemyWave.rowPosition * 0.8f - 4.4f - 8f;
            enemy.pos.Set(xPos, -yPos, 0);
            enemy.transformH.position = enemy.pos;
            return;
        }

        if (currentWave.movementMode == EnemyMovementMode.FormationHold)
        {
            timeEnemyDelay += currentWave.spawnInterval;
            enemy.movementMode = EnemyMovementMode.FormationHold;
            float xPos = GetFormationX(enemyWave.columnPosition, GetFormationColumnCount(currentWave));
            float targetY = GetFormationTargetY(enemyWave.rowPosition);
            Vector3 spawnPosition = new Vector3(xPos, 5.45f + enemyWave.rowPosition * 0.25f, 0f);
            Vector3 targetPosition = new Vector3(xPos, targetY, 0f);
            enemy.MoveToFormationHold(
                spawnPosition,
                targetPosition,
                Mathf.Max(0.2f, currentWave.pathTravelDuration),
                Mathf.Max(2f, currentWave.verticalSpeed),
                timeEnemyDelay);
            return;
        }

        if (currentWave.movementMode == EnemyMovementMode.FormationSine)
        {
            timeEnemyDelay += currentWave.spawnInterval;
            enemy.movementMode = EnemyMovementMode.FormationSine;
            float phaseOffset = enemyWave.rowPosition * 0.82f + (enemyWave.columnPosition % 2) * Mathf.PI;
            float amplitude = FormationLayout.GetSineAmplitude(GetVisibleXLimit());
            float yPos = 5.45f + enemyWave.rowPosition * 0.45f;
            enemy.MoveSineHorizontal(
                new Vector3(0f, yPos, 0f),
                GetFormationTargetY(enemyWave.rowPosition),
                Mathf.Max(0.3f, currentWave.verticalSpeed),
                Mathf.Max(3f, currentWave.pathTravelDuration),
                amplitude,
                2.65f,
                phaseOffset,
                timeEnemyDelay);
            return;
        }

        if (currentWave.movementMode == EnemyMovementMode.FormationDiagonalHold)
        {
            timeEnemyDelay += currentWave.spawnInterval;
            enemy.movementMode = EnemyMovementMode.FormationDiagonalHold;
            float laneX = GetFormationX(enemyWave.columnPosition, GetFormationColumnCount(currentWave)) * 0.68f;
            float startX = ClampToVisibleX(laneX + 0.58f);
            float targetX = ClampToVisibleX(laneX - 0.58f);
            float startY = 5.45f + enemyWave.rowPosition * 0.42f;
            float targetY = GetDiagonalFormationTargetY(enemyWave.rowPosition, enemyWave.columnPosition);
            enemy.MoveToFormationHold(
                new Vector3(startX, startY, 0f),
                new Vector3(targetX, targetY, 0f),
                Mathf.Max(0.2f, currentWave.pathTravelDuration),
                Mathf.Max(2f, currentWave.verticalSpeed),
                timeEnemyDelay);
            return;
        }

        if (currentWave.movementMode == EnemyMovementMode.FormationCircle)
        {
            timeEnemyDelay += currentWave.spawnInterval;
            enemy.movementMode = EnemyMovementMode.FormationCircle;
            int circleCount = Mathf.Max(1, currentWave.enemyList.Count);
            float angle = 360f * index / circleCount;
            float radius = FormationLayout.GetCircleRadius(GetVisibleXLimit());
            Vector3 center = new Vector3(0f, 2.1f, 0f);
            Vector3 spawnPosition = new Vector3(GetFormationX(enemyWave.columnPosition, GetFormationColumnCount(currentWave)), 5.45f + enemyWave.rowPosition * 0.28f, 0f);
            enemy.MoveCircleFormation(
                spawnPosition,
                center,
                radius,
                angle,
                Mathf.Max(0.4f, currentWave.pathTravelDuration),
                Mathf.Max(18f, currentWave.verticalSpeed),
                timeEnemyDelay);
            return;
        }

        timeEnemyDelay += currentWave.spawnInterval;
        enemy.movementMode = EnemyMovementMode.FollowPath;
        if (PathGroup.instance == null)
        {
            PathGroup pathGroup = GameObject.FindObjectOfType<PathGroup>();
            if (pathGroup != null)
            {
                pathGroup.Initialize();
            }
        }
        int pathListIndex = GetPathListIndex(indexPath);
        if (PathGroup.instance == null
            || PathGroup.instance.posPathList == null
            || pathListIndex < 0
            || pathListIndex >= PathGroup.instance.posPathList.Length)
        {
            Debug.LogError("[WaveManager] Missing PathGroup data for path " + enemyPath);
            enemy.gameObject.SetActive(false);
            GameManager.Instance.enemyPool.Store(enemy);
            return;
        }

        enemy.transformH.position = ClampToVisibleX(PathGroup.instance.posPathList[pathListIndex]);
        enemy.MoveObj.timeDelay = timeEnemyDelay;
        enemy.MoveObj.timeRun = currentWave.pathTravelDuration;
        enemy.MoveObj.pathName = indexPath == 0
            ? "Path" + enemyPath.ToString()
            : "Path" + enemyPath.ToString() + "-" + indexPath.ToString();
        enemy.StartHide();
        enemy.MoveObj.Tween();
    }



void CheckIndexPathList()
    {
        if (enemyPath <= 1)
        {
            indexPathList = 0;
            return;
        }

        indexPathList = enemyPath + 2;
    }

    private int GetPathListIndex(int laneIndex)
    {
        return laneIndex <= 0 ? indexPathList : indexPathList + laneIndex - 1;
    }

    private float GetFormationTargetY(int rowPosition)
    {
        return FormationLayout.GetTargetY(rowPosition);
    }

    private float GetDiagonalFormationTargetY(int rowPosition, int columnPosition)
    {
        return FormationLayout.GetDiagonalTargetY(rowPosition, columnPosition);
    }

    private float GetFormationX(int columnPosition, int columnCount)
    {
        return FormationLayout.GetColumnX(columnPosition, columnCount, GetVisibleXLimit());
    }

    private int GetFormationColumnCount(WaveConfigData wave)
    {
        if (wave == null || wave.enemyList == null || wave.enemyList.Count == 0)
            return 1;

        int maxColumn = 0;
        for (int i = 0; i < wave.enemyList.Count; i++)
        {
            if (wave.enemyList[i] != null)
                maxColumn = Mathf.Max(maxColumn, wave.enemyList[i].columnPosition);
        }

        return maxColumn + 1;
    }

    private Vector3 ClampToVisibleX(Vector3 position)
    {
        position.x = ClampToVisibleX(position.x);
        return position;
    }

    private float ClampToVisibleX(float x)
    {
        float visibleLimit = GetVisibleXLimit();
        return Mathf.Clamp(x, -visibleLimit, visibleLimit);
    }

    private float GetVisibleXLimit()
    {
        Camera camera = Camera.main;
        if (camera == null || !camera.orthographic)
            return 1.8f;

        const float horizontalPadding = 0.35f;
        float halfWidth = camera.orthographicSize * camera.aspect;
        return Mathf.Max(0.45f, halfWidth - horizontalPadding);
    }

    private float GetEnemyHealthScale()
    {
        return GameplayDifficultyRules.GetEnemyHealthScale(GameState.currentLevel.Value);
    }

    private float GetEnemyAttackScale()
    {
        return GameplayDifficultyRules.GetEnemyAttackScale(GameState.currentLevel.Value);
    }

    private float GetEnemyFireRateMultiplier()
    {
        return GameplayDifficultyRules.GetEnemyFireRateMultiplier(GameState.currentLevel.Value);
    }

    private bool CanEnemyShoot(WaveConfigData wave, EnemyWaveConfigData enemy)
    {
        return GameplayDifficultyRules.CanEnemyShoot(
            GameState.currentLevel.Value,
            wave.movementMode,
            enemy.rowPosition);
    }
}
