using System;
using UnityEngine;

public class CovertHealth : MonoBehaviour,IDamageable
{
    [SerializeField]
    public float salud = 100;
    private float saludActual;

    public event Action<float, float> OnHealthChanged;

    public float MaxHealth => salud;

    public float CurrentHealth => saludActual;

    void Start()
    {
        saludActual = salud;
        OnHealthChanged?.Invoke(salud, saludActual);
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
        if (saludActual <= 0) Destroy(this.gameObject);
    }

}
