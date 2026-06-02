using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System;


public class HealthZombie : MonoBehaviour, IDamageable
{
    private Image healthBarFill;

    private TextMeshProUGUI healthText;

    [SerializeField]
    private float regenPerSecond = 1f;

    [SerializeField]
    private float regenDistance = 8f;

    private LayerMask opponentMask;

    [SerializeField]
    private GameObject feedbackTextPrefab;
    
    [SerializeField]
    private GameObject puddlePrefab;

    private Transform tempContainer;

    [SerializeField]
    private float healthMax =100f;
    public float MaxHealth => healthMax;

    public float health;
    public float CurrentHealth => health;

    [HideInInspector]
    public bool isDead;

    private bool isPlayer;

    private Animator anim;
    private NavMeshAgent agent;
    private LocomotionSimpleAgent locomotion;
    private CapsuleCollider capsule;
    private Combat combat;
    private ClickToMove clickToMove;
    private EnemyAI enemyAI;
    private NavMeshObstacle obstacle;
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    public void TakeDamage(float amount) => ChangeHealth(amount);
    private void Start()
    {
        OnHealthChanged?.Invoke(MaxHealth, CurrentHealth);
    }
    private void Awake()
    {
        health = healthMax;

        isPlayer = gameObject.CompareTag("Player");

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        locomotion = GetComponent<LocomotionSimpleAgent>();
        capsule = GetComponent<CapsuleCollider>();
        combat = GetComponent<Combat>();
        clickToMove = GetComponent<ClickToMove>();
        enemyAI = GetComponent<EnemyAI>();
        obstacle = GetComponent<NavMeshObstacle>();

        if (isPlayer)
        {
            healthBarFill =
                GameObject.Find("PlayerHealthBarFill")
                .GetComponent<Image>();

            healthText =
                GameObject.Find("PlayerHealthText")
                .GetComponent<TextMeshProUGUI>();

            opponentMask = LayerMask.GetMask("Enemy");
        }
        else
        {
            healthBarFill =
                transform.Find("EnemyHealthBar/Image Fill")
                .GetComponent<Image>();

            healthBarFill.transform.parent.gameObject.SetActive(false);

            opponentMask = LayerMask.GetMask("Player");
        }

        GameObject container =
            GameObject.Find("Temp Container");

        if (container != null)
        {
            tempContainer = container.transform;
        }

        ChangeHealth(0);

        InvokeRepeating(
            nameof(Regeneration),
            1f,
            1f
        );
    }

    public void ChangeHealth(float amount)
    {
        if (isDead)
            return;

        if (feedbackTextPrefab != null && Mathf.Round(amount) != 0)
        {
            GameObject goFeedback =
                Instantiate(
                    feedbackTextPrefab,
                    transform.position,
                    Quaternion.identity,
                    tempContainer
                );

            goFeedback.name = "Feedback Text";

            goFeedback
                .GetComponent<FeedbackText>()
                .ChangeText(-amount);
        }

        health -= amount;
        health = Mathf.Clamp(health, 0f, healthMax);

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount =
                Tools.MapValues(
                    health,
                    0f,
                    healthMax,
                    0f,
                    1f
                );

            if (!isPlayer)
            {
                healthBarFill.transform.parent.gameObject.SetActive(
                    health > 0f && health < healthMax
                );
            }
        }

        if (isPlayer && healthText != null)
        {
            healthText.text =
                Mathf.Round(health)
                + "/"
                + healthMax;
        }

        if (amount < 0f)
        {
            SpawnBloodPuddle();
        }

        Debug.Log(gameObject.name + " health: " + health);
        OnHealthChanged?.Invoke(MaxHealth, CurrentHealth);
        
        if (health <= 0f && !isDead)
        {
            isDead = true;
            StartCoroutine(DeathHandler());
        }
    }

    private IEnumerator DeathHandler()
    {
        if (anim != null)
        {
            anim.SetBool("move", false);
            anim.SetBool("attack", false);
            anim.SetBool("Attack", false);
            anim.SetBool("StandBy", false);
        }

        if (agent != null)
            agent.enabled = false;

        if (locomotion != null)
            locomotion.enabled = false;

        if (capsule != null)
            capsule.enabled = false;

        if (isPlayer)
        {
            if (combat != null)
                combat.enabled = false;

            if (clickToMove != null)
                clickToMove.enabled = false;
        }
        else
        {
            if (enemyAI != null)
                enemyAI.enabled = false;

            if (obstacle != null)
                obstacle.enabled = false;
        }


        if (anim != null)
        {
            int randomDeath = UnityEngine.Random.Range(1, 12);

            anim.ResetTrigger("death");

            anim.SetInteger("deathID", randomDeath);
            anim.SetTrigger("death");

            yield return null;

            anim.ResetTrigger("death");

            AnimatorStateInfo asi =
                anim.GetCurrentAnimatorStateInfo(0);

            float elapsed = 0f;

            while (
                asi.tagHash != Animator.StringToHash("death")
                &&
                elapsed < 5f
            )
            {
                elapsed += Time.deltaTime;
                asi = anim.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }

            float waitTime =
                Mathf.Max(0f, asi.length - elapsed);

            yield return new WaitForSeconds(waitTime + 1f); 

        }

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 10f;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        yield return new WaitForSeconds(3f);

        if (isPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            OnDeath?.Invoke();
        }
    }

    private void SpawnBloodPuddle()
    {
        if (puddlePrefab == null)
            return;

        Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-0.25f, 0.25f),
            0f,
            UnityEngine.Random.Range(-0.25f, 0.25f)
        );

        Quaternion randomRotation =
            Quaternion.Euler(
                0f,
                UnityEngine.Random.Range(0f, 360f),
                0f
            );

        GameObject puddle =
            Instantiate(
                puddlePrefab,
                transform.position + randomOffset,
                randomRotation,
                tempContainer
            );

        float randomScale =
            UnityEngine.Random.Range(0.25f, 1f);

        puddle.transform.localScale =
            Vector3.one * randomScale;
    }
    private void Regeneration()
    {
        if (isDead)
            return;

        if (regenPerSecond <= 0f)
            return;

        bool isAlone =
            Physics.OverlapSphere(
                transform.position,
                regenDistance,
                opponentMask
            ).Length == 0;

        if (isAlone && health < healthMax)
        {
            ChangeHealth(-regenPerSecond);
        }
    }
}