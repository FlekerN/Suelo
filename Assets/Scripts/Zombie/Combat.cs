using UnityEngine;
using UnityEngine.AI;

public class Combat : MonoBehaviour
{
    [SerializeField]
    private Vector2 dmgRange = new Vector2(2f, 5f);

    [SerializeField]
    private float attackRadius = 0.6f;

    private bool isPlayer;

    private LayerMask opponentMask;

    private Animator anim;
    private NavMeshAgent agent;
    private Camera cam;

    private void Awake()
    {
        isPlayer = gameObject.CompareTag("Player");

        opponentMask = isPlayer
            ? LayerMask.GetMask("Enemy")
            : LayerMask.GetMask("Player");

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (!isPlayer)
            return;

        bool isStopped =
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f;

        bool canAttack =
            Input.GetMouseButton(1) &&
            isStopped;

        anim.SetBool("attack", canAttack);

        if (canAttack)
        {
            PlayerLookAtEnemy();
        }
    }

    public void ImpactEvent()
    {
        Vector3 offset =
            transform.forward * 0.5f + Vector3.up;

        Collider[] hits =
            Physics.OverlapSphere(
                transform.position + offset,
                attackRadius,
                opponentMask
            );

        if (hits.Length <= 0)
            return;

        foreach (Collider hit in hits)
        {
            // Collider destruido
            if (hit == null)
                continue;

            // HealthZombie inexistente
            HealthZombie hp =
                hit.GetComponent<HealthZombie>();

            if (hp == null)
                continue;

            // Daño aleatorio
            float dmgAmt =
                Mathf.Round(
                    Random.Range(
                        dmgRange.x,
                        dmgRange.y
                    )
                );

            // Bonus de prioridad
            if (isPlayer)
            {
                EnemyAI enemy =
                    hit.GetComponent<EnemyAI>();

                if (enemy != null)
                {
                    enemy.priorityBonus--;
                }
            }

            hp.ChangeHealth(-dmgAmt);

        }
    }

    private void PlayerLookAtEnemy()
    {
        Vector3 input = Input.mousePosition;

        Vector3 lookPoint =
            cam.ScreenToWorldPoint(
                new Vector3(input.x, input.y, 10f)
            );

        lookPoint =
            new Vector3(
                lookPoint.x,
                transform.position.y,
                lookPoint.z
            );

        Vector3 lookDirection =
            lookPoint - transform.position;

        if (lookDirection == Vector3.zero)
            return;

        Quaternion lookRot =
            Quaternion.LookRotation(lookDirection);

        transform.rotation =
            Quaternion.Lerp(
                transform.rotation,
                lookRot,
                Time.deltaTime * 10f
            );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 offset =
            transform.forward * 0.5f + Vector3.up;

        Gizmos.DrawWireSphere(
            transform.position + offset,
            attackRadius
        );
    }
}