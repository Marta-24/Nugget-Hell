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

    [Header("Configuración de Olas")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public float timeBetweenWaves = 5f;

    [Header("Condición de Victoria de Prueba")]
    [Tooltip("Número de enemigos que hay que matar para ganar.")]
    public int enemiesToKillForWin = 3;
    private int totalEnemiesKilled = 0;

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
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartCoroutine(SpawnWave());
        }
        else
        {
            Debug.Log("¡Todas las olas completadas! HAS GANADO");
            FindObjectOfType<UIManager>().ShowWinScreen();
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
        totalEnemiesKilled++;
        Debug.Log("Enemigo muerto. Total de muertes: " + totalEnemiesKilled);

        if (totalEnemiesKilled >= enemiesToKillForWin)
        {
            Debug.Log("¡Objetivo de muertes alcanzado! Mostrando pantalla de victoria.");
            FindObjectOfType<UIManager>().ShowWinScreen();

            StopAllCoroutines();
            return;
        }

        enemiesAlive--;
        if (enemiesAlive <= 0 && enemiesRemainingToSpawn == 0)
        {
            currentWaveIndex++;
            StartCoroutine(StartNextWave());
        }
    }
}