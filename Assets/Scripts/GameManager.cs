using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;
    private int enemiesRemainingToSpawn;
    private int enemiesAlive;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("GameManager ha iniciado. Empezando la primera ola...");
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        Debug.Log("StartNextWave llamado. Índice de ola actual: " + currentWaveIndex);
        if (currentWaveIndex < waves.Length)
        {
            Debug.Log("Esperando " + timeBetweenWaves + " segundos para la siguiente ola.");
            yield return new WaitForSeconds(timeBetweenWaves);

            Debug.Log("Iniciando Corutina SpawnWave.");
            StartCoroutine(SpawnWave());
        }
        else
        {
            Debug.Log("¡Todas las olas completadas! HAS GANADO");
        }
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesAlive = currentWave.enemyCount;

        for (int i = 0; i < enemiesRemainingToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
        }
    }

    void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && enemiesRemainingToSpawn == 0)
        {
            currentWaveIndex++;
            StartCoroutine(StartNextWave());
        }
    }
}