using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    [Tooltip("El objeto que la cámara debe seguir (el jugador).")]
    public Transform target;

    [Tooltip("La distancia y ángulo de la cámara respecto al jugador.")]
    public Vector3 offset = new Vector3(0, 10, -7);

    [Tooltip("Suavidad del movimiento. Un valor más bajo es más lento y suave.")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("La cámara no tiene un objetivo (target) asignado.");
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}