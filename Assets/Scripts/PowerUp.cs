using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Ketchup, Mustard }
    public PowerUpType type;

    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.ActivatePowerUp(type);
                Destroy(gameObject);
            }
        }
    }
}