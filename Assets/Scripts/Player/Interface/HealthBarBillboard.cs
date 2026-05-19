using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        // Cache de la cámara principal. Si usas Cinemachine u otra
        // cámara dinámica, sustituye esto por una referencia inyectada.
        if (Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // LateUpdate asegura que la cámara ya actualizó su posición este frame.
        if (_cameraTransform == null) return;

        // El Canvas mira hacia la cámara: su normal apunta al observador.
        transform.LookAt(
            transform.position + _cameraTransform.rotation * Vector3.forward,
            _cameraTransform.rotation * Vector3.up
        );
    }
}
