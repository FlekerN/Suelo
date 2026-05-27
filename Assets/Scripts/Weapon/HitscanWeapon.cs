using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HitscanWeapon : Weapon
{
    public float damage = 20f;
    public float range = 100f;
    public float impactBulletForce = 36f;

    public Transform cameraTransform;
    public GameObject HitInstance;

    [SerializeField] private LayerMask shootMask;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Pew pew! (Raycast)");

        if (anim != null)
        {
            anim.SetTrigger("Shoot");
        }

        Debug.DrawRay(
            cameraTransform.position,
            cameraTransform.forward * range,
            Color.red,
            1.5f);

        if (Physics.Raycast(
            cameraTransform.position,
            cameraTransform.forward,
            out RaycastHit hit,
            range,
            shootMask,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"Impacto en: {hit.collider.name}");

            GameObject objeto = Instantiate(
                HitInstance,
                hit.point,
                Quaternion.identity);

            objeto.transform.parent = hit.collider.transform;

            IDamageable health = hit.collider.GetComponent<IDamageable>();

            if (health == null)
                health = hit.collider.GetComponentInParent<IDamageable>();

            health?.TakeDamage(damage);

            if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
            {
                Rigidbody rb = hit.rigidbody;

                rb.AddForceAtPosition(
                    -hit.normal * impactBulletForce,
                    hit.point,
                    ForceMode.Impulse);
            }
        }
    }
}