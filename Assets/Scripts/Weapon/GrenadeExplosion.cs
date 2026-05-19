using System;
using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    public float tiempoExplosion = 3f;
    public float fuerzaExplosion = 700f;
    public float radioExplosion = 5f;
    public float fuerzaVertical = 1f;

    public GameObject ExplosionPrefab;

    private void Start()
    {
        StartCoroutine(Temporizador());
    }

    IEnumerator Temporizador() 
    {
        yield return new WaitForSeconds(tiempoExplosion);
        Explode();
    }
    void Explode() 
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, radioExplosion);
        foreach (Collider nearbyObject in collider)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic) 
            {
                rb.AddExplosionForce(fuerzaExplosion,transform.position,radioExplosion,fuerzaVertical,ForceMode.Impulse);
            }
        }

        GameObject fx = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(fx, 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
