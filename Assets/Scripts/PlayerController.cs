using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Configuración de Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Salud y UI")]
    public int maxHealth = 10;
    public UIManager uiManager;

    [Header("Capa del Suelo")]
    public LayerMask groundMask;

    private int currentHealth;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Camera mainCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (uiManager != null)
        {
            uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveInput != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveInput), rotationSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void FixedUpdate() { rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime); }
    void Fire()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 projectileDirection = hitPoint - firePoint.position;

            Quaternion projectileRotation = Quaternion.LookRotation(projectileDirection);

            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, projectileRotation);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        uiManager.UpdatePlayerHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡El jugador ha muerto! GAME OVER");
        this.enabled = false;

        if (uiManager != null)
        {
            uiManager.ShowLoseScreen();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        uiManager.UpdatePlayerHealth(currentHealth, maxHealth);
        Debug.Log("Jugador curado. Vida actual: " + currentHealth);
    }
}