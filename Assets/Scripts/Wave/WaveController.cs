using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    // first - direction from which enemy spawns, second - time in which enemy spawns
    [Serializable]
    private struct EnemySpawnInfo
    {
        public Direction direction;
        public float spawnTime;
    }

    [SerializeField] private GameObject getReadyTextObject;
    [SerializeField] private GameObject nextWayTextObject;
    [SerializeField] private GameObject simpleEnemy1Prefab;
    [SerializeField] private GameObject simpleEnemy2Prefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject trigger;

    [SerializeField] private float preparationTime = 3f;
    [SerializeField] private Transform leftSpawnPosition;
    [SerializeField] private Transform rightSpawnPosition;
    [SerializeField] private List<GameObject> invisibleWalls = new();
    [SerializeField] private List<WaveWallsCollideController> invisibleWallsCollideControllers = new();
    [SerializeField] private List<EnemySpawnInfo> waveEnemySpawnInfos = new();

    private PlayerLifecycleHandler playerLifecycleHandler;
    private Transform cameraFollow;
    private bool shouldEndWave = false;
    public bool IsWaveGoing {get; private set;} = false;

    private int livingEnemiesCount = 0;

    void Awake()
    {
        foreach (var wall in invisibleWalls)
            invisibleWallsCollideControllers.Add(wall.GetComponent<WaveWallsCollideController>());
        SetWallsActive(true);
        playerLifecycleHandler = FindAnyObjectByType<PlayerLifecycleHandler>();
        nextWayTextObject.SetActive(false);
    }

    void OnEnable()
    {
        playerLifecycleHandler.Died += OnPlayerDied;
    }

    void OnDisable()
    {
        playerLifecycleHandler.Died -= OnPlayerDied;
    }

    public void Initialize()
    {
        SetCameraStatic();
        StartCoroutine(StartPreparationRoutine());
        SetWallsActive(true);
        //SetWallsCollideControllerEnable(false);
        trigger.SetActive(false);
        IsWaveGoing = true;
    }

    void Update()
    {
        if (EnemyDieCounter.count == 6 && IsWallsActive())
        {
            SetWallsActive(false);
            EnemyDieCounter.count = 0;
        }
    }

    private void OnPlayerDied()
    {
        EndWave(true);
        //SetWallsCollideControllerEnable(true);
    }

    // private void SetWallsCollideControllerEnable(bool enable)
    // {
    //     foreach (var controller in invisibleWallsCollideControllers)
    //     {
    //         Debug.Log(controller);
    //         controller.enabled = enable;
    //     }
    // }

    private bool IsWallsActive()
    {
        bool active = true;
        foreach (var wall in invisibleWalls)
            active &= wall.activeInHierarchy;
        return active;
    }

    private void SetWallsActive(bool active)
    {
        foreach (var wall in invisibleWalls)
            wall.SetActive(active);
    }

    private IEnumerator StartPreparationRoutine()
    {
        getReadyTextObject.SetActive(true);
        yield return new WaitForSeconds(preparationTime);
        StartWave();
        getReadyTextObject.SetActive(false);
    }

    private void StartWave()
    {
        for (int i = 0; i < waveEnemySpawnInfos.Count; ++i)
        {
            var info = waveEnemySpawnInfos[i];
            Vector2? position = GetSpawnPositionFromDirection(info.direction);
            Debug.Assert(position != null, "WaveController: position not left or right");

            bool isLastEnemy = i == waveEnemySpawnInfos.Count - 1;
            StartCoroutine(SpawnRandomSimpleEnemyRoutine(position.Value, info.spawnTime, isLastEnemy));
        }
    }

    private void EndWave(bool loosing)
    {
        SetCameraFollow();
        SetWallsActive(false);
        StopAllCoroutines();
        IsWaveGoing = false;
        if (loosing)
        {
            SetWallsActive(false);
            trigger.SetActive(true);
        }
        else
            nextWayTextObject.SetActive(true);
        Debug.Log("wave ended");
    }

    private Vector2? GetSpawnPositionFromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Left => leftSpawnPosition.position,
            Direction.Right => rightSpawnPosition.position,
            _ => null
        };
    }

    private void SpawnRandomSimpleEnemy(Vector2 position) // TODO: нарушение SRP
    {
        var enemyPrefab = UnityEngine.Random.value >= 0.5f ? simpleEnemy1Prefab : simpleEnemy2Prefab;
        GameObject enemyObject = Instantiate(enemyPrefab, position, Quaternion.identity);
        var enemyController = enemyObject.GetComponent<EnemyController>();
        var enemy = enemyObject.GetComponent<Enemy>();
        enemyController.DetectionRadius = 20f;
        enemyController.DestroyOnPlayerRevive = true;
        enemy.DestroyOnDeath = true;
        livingEnemiesCount += 1;

        enemy.Died += OnEnemyDied;
    }

    private IEnumerator SpawnRandomSimpleEnemyRoutine(Vector2 position, float seconds, bool isLastEnemy) // TODO: нарушение SRP
    {
        yield return new WaitForSeconds(seconds);
        shouldEndWave = isLastEnemy;
        SpawnRandomSimpleEnemy(position);
    }

    private void OnEnemyDied()
    {
        livingEnemiesCount -= 1;
        if (livingEnemiesCount == 0 && shouldEndWave)
            EndWave(false);
    }

    private void SetCameraFollow()
    {
        if (cameraFollow != null)
            cinemachineCamera.Follow = cameraFollow;
    }

    private void SetCameraStatic()
    {
        cameraFollow = cinemachineCamera.Follow;
        cinemachineCamera.Follow = null;
    }
}
