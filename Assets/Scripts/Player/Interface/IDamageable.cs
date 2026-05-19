using System;
using UnityEngine;

public interface IDamageable 
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    event Action<float, float> OnHealthChanged;
    public void TakeDamage(float dmg);
}
