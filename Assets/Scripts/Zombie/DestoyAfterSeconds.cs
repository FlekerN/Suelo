using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 8f;

    private void Start()
    {
        Destroy(gameObject, seconds);
    }
}