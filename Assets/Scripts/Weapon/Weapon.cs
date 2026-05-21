using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponName;
    public float fireRate = 0.5f;
    protected float nextFireTime = 0f;

    // Método que el jugador llamará
    public virtual void TryAttack()
    {
        if (Time.time >= nextFireTime)
        {
            PerformAttack();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Cada arma específica definirá CÓMO ataca aquí
    protected abstract void PerformAttack();
    
}

