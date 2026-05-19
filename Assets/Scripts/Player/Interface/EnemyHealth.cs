using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    public float salud = 60;
    private float saludActual;

    RagdollAnimator rgd;

    public event Action<float, float> OnHealthChanged;

    public float MaxHealth => salud;

    public float CurrentHealth => saludActual;

    void Start() 
    {
        rgd = GetComponent<RagdollAnimator>();
        saludActual = salud;

        OnHealthChanged?.Invoke(salud,saludActual);
    }
    public void TakeDamage(float dmg)
    {
        saludActual -= dmg;
        OnHealthChanged?.Invoke(salud, saludActual);

        if (saludActual <= 0)
        {
            Die();
        }
    }
    private void Die() 
    {
        if (rgd != null) rgd.EnableRagdoll();
    }
}