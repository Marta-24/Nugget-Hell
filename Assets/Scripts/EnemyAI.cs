using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
            GameManager.instance.OnEnemyDied(this.gameObject);
        }
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TakeDamage(1);
            }

            StartCoroutine(HandleCollision());
        }
    }

    IEnumerator HandleCollision()
    {
        agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        if (this != null)
        {
            agent.enabled = true;

        }
    }
}