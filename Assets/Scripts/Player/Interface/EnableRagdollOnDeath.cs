using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnableRagdollOnDeath : MonoBehaviour
{
    private IDamageable damageable;
    private RagdollAnimator rgd;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
        rgd = GetComponent<RagdollAnimator>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += EnableRagdoll;
    }

    private void OnDisable()
    {
        damageable.OnDeath -= EnableRagdoll;
    }

    private void EnableRagdoll()
    {
        if (rgd != null)
            rgd.EnableRagdoll();
    }
}