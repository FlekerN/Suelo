using UnityEngine;

public class ParabolicWeapon : Weapon
{
    public GameObject grenadePrefab;
    public Transform firePoint;
    public float throwForce = 10f;
    public float upwardForce = 1.5f;

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Granada va!");

        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);

        if (grenade.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Combinamos fuerza frontal y elevación
            Vector3 forceToAdd = (firePoint.forward * throwForce) + (firePoint.up * upwardForce);

            // Usamos Impulse porque es un golpe de fuerza instantáneo
            rb.AddForce(forceToAdd, ForceMode.Impulse);
        }
    }
}
