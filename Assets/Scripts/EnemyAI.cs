using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 3;

    [Header("UI de Salud")]
    public GameObject healthBarUI;
    public RectTransform healthBarFillRect;

    private int currentHealth;
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarUI.SetActive(false);

        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (healthBarUI.activeSelf == false)
        {
            healthBarUI.SetActive(true);
        }

        float healthPercent = (float)currentHealth / maxHealth;
        healthBarFillRect.localScale = new Vector3(healthPercent, 1, 1);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnEnemyDied();
        }
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemigo ha chocado con: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("El objeto tiene la etiqueta 'Player'. Intentando hacer daño...");

            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                Debug.Log("PlayerController encontrado. Haciendo 1 de daño.");
                player.TakeDamage(1);
            }
            else
            {
                Debug.LogWarning("¡ERROR! El objeto tiene la etiqueta 'Player' pero no se encontró el script PlayerController.");
            }
        }
    }
}