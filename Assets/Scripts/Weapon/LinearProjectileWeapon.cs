using UnityEngine;

public class LinearProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public Transform firePoint; // El cañón del arma
    public float projectileSpeed = 50f;

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Fum! (Proyectil)");

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        if (proj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Le damos velocidad en la dirección en la que mira el cañón
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }
    }
}