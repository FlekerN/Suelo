using System.Collections;
using UnityEngine;

public class GrenadeExplosion : Explosive
{
    [SerializeField] private float tiempoExplosion = 3f;

    private void Start()
    {
        StartCoroutine(Temporizador());
    }

    private IEnumerator Temporizador()
    {
        yield return new WaitForSeconds(tiempoExplosion);
        Explode();
    }
}
