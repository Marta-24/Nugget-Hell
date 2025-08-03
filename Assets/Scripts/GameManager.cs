using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs para Debug")]
    public GameObject ketchupPrefab_Debug;
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

    public GameObject mustardPrefab_Debug;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                Debug.Log("DEBUG: Jugador recibe 1 de daño.");
                player.TakeDamage(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && enemyPrefab != null)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
                Debug.Log("DEBUG: Spawneando enemigo en " + spawnPosition);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && ketchupPrefab_Debug != null)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                Debug.Log("DEBUG: Spawneando Ketchup en " + spawnPosition);
                Instantiate(ketchupPrefab_Debug, spawnPosition, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null && mustardPrefab_Debug != null)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                Debug.Log("DEBUG: Spawneando Mostaza en " + spawnPosition);
                Instantiate(mustardPrefab_Debug, spawnPosition, Quaternion.identity);
            }
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