using UnityEngine;

public static class NoiseSystem
{
    public static void EmitNoise(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);

        foreach (Collider hit in hits)
        {
            EnemyBrain enemy = hit.GetComponentInParent<EnemyBrain>();
            enemy?.HearNoise(position);
        }
    }
}