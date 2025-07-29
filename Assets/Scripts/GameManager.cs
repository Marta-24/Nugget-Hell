using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

    [Header("Configuración de Olas")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = -1;
    private bool isSpawningWave = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    public static GameManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (!isSpawningWave && activeEnemies.Count == 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Length)
        {
            isSpawningWave = true;
            StartCoroutine(SpawnWave());
        }
        else if (!isSpawningWave)
        {
            Debug.Log("¡Todas las olas completadas! HAS GANADO");
            FindObjectOfType<UIManager>().ShowWinScreen();
            this.enabled = false;
        }
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        }
        isSpawningWave = false;
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        activeEnemies.Add(newEnemy);

    }

    public void OnEnemyDied(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}