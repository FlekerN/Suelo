using UnityEngine;
[RequireComponent(typeof(Animator))]

public class MeleeWeapon : Weapon
{
    public float damage = 50f;
    public float attackRange = 1f;
    public float attackRadius = 1f; // Lo "gordo" que es el golpe
    public Transform cameraTransform;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: (Navajazo)");

        if (anim != null) 
        {
            anim.SetTrigger("Slash");
        }
        // Calculamos el punto frente a la cámara
        Vector3 attackPoint = cameraTransform.position + cameraTransform.forward * attackRange;

        // Detectamos todo lo que esté en esa pequeña esfera
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, attackRadius);

        foreach (var hit in hitColliders)
        {
            IDamageable health = hit.GetComponent<IDamageable>();
            if(health == null)  health = hit.GetComponentInParent<IDamageable>();
            health?.TakeDamage(damage);

            if (hit.attachedRigidbody != null && !hit.attachedRigidbody.isKinematic) 
            {
                Rigidbody rb = hit.attachedRigidbody;
                Vector3 forceDirection = (hit.transform.position - attackPoint).normalized;

                rb.AddForceAtPosition(forceDirection * 50f, attackPoint ,ForceMode.Impulse);
            }
        }
    }
    // Útil para que los alumnos vean el área de ataque en la vista de escena
    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 attackPoint = cameraTransform.position + cameraTransform.forward * attackRange;
            Gizmos.DrawWireSphere(attackPoint, attackRadius);
        }
    }
}
