using UnityEngine;

public class ExplodeOnDeath : Explosive
{
    private IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += ExplodeObject;
    }

    private void OnDisable()
    {
        damageable.OnDeath -= ExplodeObject;
    }
    private void ExplodeObject()
    {
       Explode();
    }
}