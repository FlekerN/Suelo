using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform eyes;
    [SerializeField] private float viewDistance = 12f;
    [SerializeField] private float viewAngle = 70f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    public bool CanSeeTarget(out Transform target)
    {
        target = null;

        Collider[] hits = Physics.OverlapSphere(eyes.position, viewDistance, targetMask);

        foreach (Collider hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - eyes.position).normalized;
            float angle = Vector3.Angle(eyes.forward, dirToTarget);

            if (angle > viewAngle * 0.5f) continue;

            float distance = Vector3.Distance(eyes.position, hit.transform.position);

            if (!Physics.Raycast(eyes.position, dirToTarget, distance, obstacleMask))
            {
                target = hit.transform;
                return true;
            }
        }

        return false;
    }
}