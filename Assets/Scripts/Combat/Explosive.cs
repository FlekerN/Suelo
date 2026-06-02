using UnityEngine;

public abstract class Explosive : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] protected float fuerzaExplosion = 700f;
    [SerializeField] protected float radioExplosion = 5f;
    [SerializeField] protected float fuerzaVertical = 1f;
    [SerializeField] protected float damage = 200f;
    [SerializeField] protected GameObject explosionPrefab;

    private bool hasExploded;

    public virtual void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radioExplosion);

        foreach (Collider nearbyObject in colliders)
        {
            IDamageable health = nearbyObject.GetComponent<IDamageable>();
            if(health == null)  health = nearbyObject.GetComponentInParent<IDamageable>();
            health?.TakeDamage(damage);

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null && !rb.isKinematic)
            {
                rb.AddExplosionForce(
                    fuerzaExplosion,
                    transform.position,
                    radioExplosion,
                    fuerzaVertical,
                    ForceMode.Impulse
                );
            }
        }

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}