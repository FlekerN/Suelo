using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cinemachineCameraTransform;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, cinemachineCameraTransform.eulerAngles.y, 0);
    }
}
