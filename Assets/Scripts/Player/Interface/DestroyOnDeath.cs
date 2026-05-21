using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class DestroyOnDeath : MonoBehaviour
{
    private IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += DestroyObject;
    }

    private void OnDisable()
    {
        damageable.OnDeath -= DestroyObject;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}