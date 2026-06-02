using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    private NavMeshAgent agent;
    private Animator anim;

    [Header("Rangos")]
    [Range(5, 25)]
    [SerializeField] 
    private float chaseRange = 12f;
    public float attackRange;

    [Header("Patrulla")]
    [SerializeField] private float patrolRadius = 5f;

    [SerializeField]
    [Range(0, 2f)]
    public float meleeRange = 0.5f;

    [SerializeField]
    private Vector2 idleDelay = new Vector2(3f, 8f);

    [SerializeField, Range(3f, 12f)]
    private float standbyRange = 8f;

    private Vector3 standbyPos;

    [HideInInspector]
    public float distanceToPlayer;

    private LayerMask losMask;

    private Transform crumb;

    private Vector3 lastPlayerPosition;

    private float lastDestinationCalculation;

    [SerializeField]
    private float rotationSpeed = 5f;

    private Vector3 startPosition;
    private Vector3 patrolPosition;

    private float idleTimeOut;
    private float idleCounter;


    public EnemyState state;

    private NavMeshObstacle obstacle;

    [HideInInspector]
    public bool isAttackPriority;
    public int priorityBonus = 0;

    private bool isWaitingToEnableAgent;

    private void OnValidate()
    {
        chaseRange = Mathf.Round(chaseRange);
    }

    private void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        losMask = ~LayerMask.GetMask("Player", "Enemy", "Breadcrumb","Interacction");

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró el jugador.");
            enabled = false;
            return;
        }

        startPosition = transform.position;

        attackRange =
            player.GetComponent<NavMeshAgent>().radius
            + agent.radius
            + meleeRange;

        state = EnemyState.Idle;

        Idle();
    }

    private void Update()
    {
        if (player == null)
        {
            state = EnemyState.Idle;
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        crumb = GetBreadcrumb();

        if (state == EnemyState.Standby && !isAttackPriority)
        {
            idleCounter = idleTimeOut;

            if (distanceToPlayer > standbyRange)
            {
                state = EnemyState.Chase;
            }
        }
        else if (distanceToPlayer <= attackRange)
        {
            idleCounter = idleTimeOut;

            if (isAttackPriority)
            {
                state = EnemyState.Attack;
            }
            else
            {
                state = EnemyState.Standby;
            }
        }
        else if ((distanceToPlayer <= chaseRange && CheckLineOfSight(player)) || crumb != null)
        {
            idleCounter = idleTimeOut;
            state = EnemyState.Chase;
        }

        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Standby:
                Standby();
                break;
        }

        Debug.Log(state);
    }

    private void Idle()
    {
        ToggleAgent(false);
        // Reducimos contador
        idleCounter -= Time.deltaTime;

        // Cuando termina el tiempo Idle
        if (idleCounter <= 0f)
        {
            idleCounter = 0f;

            // Nuevo tiempo aleatorio
            idleTimeOut =
                Random.Range(idleDelay.x, idleDelay.y);

            idleCounter = idleTimeOut;

            // Nueva posición aleatoria
            patrolPosition =
                startPosition
                + Random.insideUnitSphere * patrolRadius;

            // Mantener misma altura
            patrolPosition.y = startPosition.y;

            standbyPos = SetStandbyPosition();

            state = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        ToggleAgent(true);

        if (!agent.isActiveAndEnabled)
            return;

        agent.stoppingDistance = 0f;

        if (agent.destination != patrolPosition)
        {
            agent.SetDestination(patrolPosition);

            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(transform.position);
                patrolPosition = agent.destination;
                return;
            }
        }

        if (!agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            idleCounter = 0f;
            state = EnemyState.Idle;
        }
    }

    private void Attack()
    {
        Debug.Log("sisisisi");
        if (player == null)
        {
            state = EnemyState.Idle;
            return;
        }
        ToggleAgent(false);

        // SOLO ataca si tiene prioridad
        if (isAttackPriority)
        {
            FacePlayer();

            Debug.Log("Attack");
        }
        else
        {
            state = EnemyState.Chase;
        }
    }

    private void Chase()
    {
        ToggleAgent(true);

        if (!agent.isActiveAndEnabled)
            return;

        standbyPos = SetStandbyPosition();

        if (CheckLineOfSight(player))
        {
            agent.stoppingDistance = attackRange;

            bool playerMoved = player.position != lastPlayerPosition;

            bool recalculatePath =
                Time.timeSinceLevelLoad > lastDestinationCalculation + 0.5f;

            if (playerMoved || recalculatePath)
            {
                agent.SetDestination(player.position);

                lastPlayerPosition = player.position;

                lastDestinationCalculation = Time.timeSinceLevelLoad;
            }
        }
        else if (crumb != null)
        {
            agent.stoppingDistance = 0f;

            if (agent.destination != crumb.position)
            {
                agent.SetDestination(crumb.position);
            }
        }

        idleCounter = idleTimeOut;
    }

    private bool CheckLineOfSight(Transform target)
    {
        Vector3 myPos = transform.position + Vector3.up;

        Vector3 targetPos = new Vector3(
            target.position.x,
            myPos.y,
            target.position.z
        );

        float rayDistance =
            Vector3.Distance(myPos, targetPos);

        Vector3 direction =
            (targetPos - myPos).normalized;

        RaycastHit hit;

        if (Physics.Raycast(
            myPos,
            direction,
            out hit,
            rayDistance,
            losMask,
            QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(
                myPos,
                direction * hit.distance,
                Color.red
            );

            return false;
        }
        else
        {
            Debug.DrawRay(
                myPos,
                direction * rayDistance,
                Color.green
            );

            return true;
        }
    }

    private Transform GetBreadcrumb()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                chaseRange
            );

        List<Breadcrumb> crumbsList =
            new List<Breadcrumb>();

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Breadcrumb"))
            {
                Breadcrumb crumb =
                    hit.GetComponent<Breadcrumb>();

                if (crumb != null)
                {
                    crumbsList.Add(crumb);
                }
            }
        }

        crumbsList.Sort(
            (a, b) => b.lifespan.CompareTo(a.lifespan)
        );

        foreach (Breadcrumb breadcrumb in crumbsList)
        {
            if (CheckLineOfSight(breadcrumb.transform))
            {
                return breadcrumb.transform;
            }
        }

        return null;
    }

    private void ToggleAgent(bool isOn)
    {
        // ACTIVAR AGENT
        if (isOn)
        {
            // Esperar 1 frame
            if (isWaitingToEnableAgent)
            {
                agent.enabled = true;

                isWaitingToEnableAgent = false;

                return;
            }

            // Si obstacle está activo
            if (obstacle.enabled)
            {
                obstacle.enabled = false;

                isWaitingToEnableAgent = true;
            }
        }

        // ACTIVAR OBSTACLE
        else
        {
            if (agent.enabled)
            {
                agent.enabled = false;
            }

            if (!obstacle.enabled)
            {
                obstacle.enabled = true;
            }
        }
    }
    
    private void FacePlayer()
    {
        if (player == null)
            return;


        Vector3 lookDirection =
            player.position - transform.position;


        if (lookDirection == Vector3.zero)
            return;
        lookDirection.y = 0f;

        Quaternion lookRot =
            Quaternion.LookRotation(lookDirection);

        transform.rotation =
            Quaternion.Lerp(
                transform.rotation,
                lookRot,
                Time.deltaTime * rotationSpeed
            );
    }

    private Vector3 SetStandbyPosition()
    {
        Vector3 basePos = player.position;

        Vector3 myPosition =
            basePos + Random.insideUnitSphere * standbyRange;

        myPosition.y = transform.position.y;

        int attempts = 0;

        while (
            attempts < 25 &&
            Vector3.Distance(myPosition, player.position)
            < attackRange)
        {
            myPosition =
                basePos + Random.insideUnitSphere * standbyRange;

            myPosition.y = transform.position.y;

            attempts++;
        }

        return (attempts < 25)
            ? myPosition
            : standbyPos;
    }

    private void Standby()
    {
        ToggleAgent(true);

        if (!agent.isActiveAndEnabled)
            return;

        if (agent.destination != standbyPos)
        {
            agent.stoppingDistance = 0f;

            agent.SetDestination(standbyPos);

            standbyPos = agent.destination;
        }

        float remainingDistance =
            Vector3.Distance(transform.position, standbyPos);

        bool isStandbyIdle =
            (Vector3.Distance(transform.position, player.position)
            <= chaseRange)
            || (crumb != null);

        if (isStandbyIdle)
        {
            FacePlayer();
        }
    }

}