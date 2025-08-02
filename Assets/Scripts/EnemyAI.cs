using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Loot")]
    [Range(0, 1)]
    public float dropChance = 0.25f;
    public GameObject ketchupPrefab;

    [Header("Salud")]
    public int maxHealth = 3;

    [Header("UI de Salud")]
    public GameObject healthBarUI;
    public RectTransform healthBarFillRect;

    [Header("Combate")]
    public float attackCooldown = 2f;
    public float knockbackForce = 5f;

    private int currentHealth;
    private Transform target;
    private NavMeshAgent agent;
    private bool canAttack = true;
    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarUI.SetActive(false);

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    void Update()
    {
        if (target != null && agent.enabled)
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
        if (Random.value <= dropChance)
        {
            if (ketchupPrefab != null)
            {
                Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
            }
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.OnEnemyDied(this.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Attack(collision.gameObject));
        }
    }

    IEnumerator Attack(GameObject playerObject)
    {
        canAttack = false;

        if (playerObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(1);
        }

        if (rb != null)
        {
            Vector3 direction = (transform.position - playerObject.transform.position).normalized;
            direction.y = 0;
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}