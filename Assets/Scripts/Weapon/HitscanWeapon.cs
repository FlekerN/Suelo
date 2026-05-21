using UnityEngine;
[RequireComponent(typeof(Animator))]

public class HitscanWeapon : Weapon
{
    public float damage = 20f;
    public float range = 100f;
    public float impactBulletForce = 36f;
    public Transform cameraTransform; // Referencia a la cámara para apuntar
    public GameObject HitInstance;
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
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * range, Color.red, 1.5f);
        // Lanzamos el rayo desde la cámara
        if (Physics.Raycast(
            cameraTransform.position, //origen
            cameraTransform.forward, //direccion
            out RaycastHit hit,  //informacion del impacto
            range))                 //distancia
        {
            Debug.Log($"Impacto en: {hit.collider.name}");
            GameObject objeto = Instantiate(HitInstance, hit.point, Quaternion.identity);
            objeto.transform.parent = hit.collider.transform;

            IDamageable health = hit.collider.GetComponent<IDamageable>();
            if(health == null)  health = hit.collider.GetComponentInParent<IDamageable>();
            health?.TakeDamage(damage);
            
            if (hit.rigidbody != null && !hit.rigidbody.isKinematic) 
            {
                Rigidbody rb = hit.rigidbody;
                
                rb.AddForceAtPosition(-hit.normal * impactBulletForce, hit.point, ForceMode.Impulse);
            }

        }
    }
}