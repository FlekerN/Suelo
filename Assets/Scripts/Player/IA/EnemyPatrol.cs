using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float arriveDistance = 0.5f;

    private int currentIndex;

    public void Patrol(NavMeshAgent agent)
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= arriveDistance)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentIndex].position);
    }
}