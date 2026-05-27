using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : Explosive
{
    private IDamageable damageable;
    [SerializeField] private float tiempoExplosion = 0.4f;

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
       StartCoroutine(Temporizador());
    }
    private IEnumerator Temporizador()
    {
        yield return new WaitForSeconds(tiempoExplosion);
        Explode();
    }
}