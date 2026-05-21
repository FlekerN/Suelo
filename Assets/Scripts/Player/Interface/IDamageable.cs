using System;

public interface IDamageable
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    event Action<float, float> OnHealthChanged;
    event Action OnDeath;

    void TakeDamage(float dmg);
}