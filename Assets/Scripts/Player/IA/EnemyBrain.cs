using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(IDamageable))]
public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private float chaseDistance = 15f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float suspiciousTime = 3f;

    private EnemyState state = EnemyState.Patrol;
    private NavMeshAgent agent;
    private EnemyPatrol patrol;
    private EnemyVision vision;
    private IDamageable damageable;

    private Transform target;
    private Vector3 lastKnownPosition;
    private float suspiciousTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<EnemyPatrol>();
        vision = GetComponent<EnemyVision>();
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += Die;
    }

    private void OnDisable()
    {
        damageable.OnDeath -= Die;
    }

    private void Update()
    {
        if (state == EnemyState.Dead) return;

        if (vision.CanSeeTarget(out Transform seenTarget))
        {
            target = seenTarget;
            lastKnownPosition = target.position;
            state = EnemyState.Chase;
        }

        switch (state)
        {
            case EnemyState.Patrol:
                patrol.Patrol(agent);
                break;

            case EnemyState.Suspicious:
                SearchLastPosition();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    public void HearNoise(Vector3 noisePosition)
    {
        if (state == EnemyState.Dead) return;

        lastKnownPosition = noisePosition;
        suspiciousTimer = suspiciousTime;
        state = EnemyState.Suspicious;
    }

    private void SearchLastPosition()
    {
        agent.SetDestination(lastKnownPosition);

        if (!agent.pathPending && agent.remainingDistance < 0.7f)
        {
            suspiciousTimer -= Time.deltaTime;

            if (suspiciousTimer <= 0f)
                state = EnemyState.Patrol;
        }
    }

    private void Chase()
    {
        if (target == null)
        {
            state = EnemyState.Suspicious;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackDistance)
        {
            state = EnemyState.Attack;
            return;
        }

        if (distance > chaseDistance)
        {
            target = null;
            state = EnemyState.Suspicious;
            return;
        }

        lastKnownPosition = target.position;
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        if (target == null)
        {
            state = EnemyState.Suspicious;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            state = EnemyState.Chase;
            return;
        }

        agent.ResetPath();

        // Aquí luego puedes meter un EnemyAttack separado.
        Debug.Log("Atacando al jugador");
    }

    private void Die()
    {
        state = EnemyState.Dead;
        agent.enabled = false;
    }
}
