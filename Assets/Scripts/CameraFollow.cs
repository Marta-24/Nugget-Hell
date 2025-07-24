using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuraci�n de C�mara")]
    [Tooltip("El objeto que la c�mara debe seguir (el jugador).")]
    public Transform target;

    [Tooltip("La distancia y �ngulo de la c�mara respecto al jugador.")]
    public Vector3 offset = new Vector3(0, 10, -7);

    [Tooltip("Suavidad del movimiento. Un valor m�s bajo es m�s lento y suave.")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("La c�mara no tiene un objetivo (target) asignado.");
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}