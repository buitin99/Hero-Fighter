using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private AnimationCurve knockBackCuvre;
    [SerializeField] private float standUpTime;
    private float health;
    private bool destroyed;
    private int stepBackHash;
    private int hitHash;
    private int knockHash;
    private int deathHash;
    private int standUpHash;
    public bool knockBack { get; private set; }
    private float knockTimer;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;
    private Collider ColliderGameObject;
    private GameManager gameManager;

    public event Action<Vector3, float, AttackType> OnTakeDamageStart;
    public event Action OnTakeDamageEnd;
    [SerializeField] private HealthBarRennder healthBarRennder = new HealthBarRennder();
    private UIMenu uI;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        ColliderGameObject = GetComponent<Collider>();
        stepBackHash = Animator.StringToHash("StepBack");
        hitHash = Animator.StringToHash("Hit");
        knockHash = Animator.StringToHash("Knock");
        deathHash = Animator.StringToHash("death");
        standUpHash = Animator.StringToHash("StandUp");
        uI = FindObjectOfType<UIMenu>();

    }

    private void Start()
    {
        SetInitHealthBar(maxHealth);
        health = maxHealth;
    }

    private void LateUpdate() {
        healthBarRennder.UpdateHealthBarRotation();
    }


    private void Update()
    {
        if (knockBack)
        {
            knockTimer += Time.deltaTime;
            float speed = knockBackCuvre.Evaluate(knockTimer) * 10f;
            agent.Move(-transform.forward.normalized * speed * Time.deltaTime);
        }
    }

    public void TakeDamgae(Vector3 hitPoint, float damage, AttackType attackType)
    {
        if (!destroyed)
        {
            uI.DisplayHitPoint();
            health -= damage;
            healthBarRennder.UpdateHealthBarValue(health);
            if (health <= 0)
            {
                destroyed = true;
                Destroy(attackType);
                return;
            }
            HandleHitReaction(hitPoint, attackType);
            OnTakeDamageStart?.Invoke(hitPoint, damage, attackType);

        }
    }

    private void HandleHitReaction(Vector3 hitPoint, AttackType attackType)
    {
        Quaternion rot = Quaternion.LookRotation(hitPoint - transform.position);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = rot;
        if (attackType == AttackType.light)
        {
            animator.SetTrigger(hitHash);
        }
        else
        {
            knockBack = true;
            animator.SetTrigger(knockHash);
            animator.ResetTrigger(standUpHash);
        }
    }

    public void TakeDamagaEnd()
    {
        OnTakeDamageEnd?.Invoke();
    }

    public void KnockEnd()
    {
        knockBack = false;
        knockTimer = 0;
        Invoke("StandUp", standUpTime);
    }

    private void StandUp()
    {
        animator.SetTrigger(standUpHash);
    }

    public void SetInitHealthBar(float health)
    {
        healthBarRennder.CreateHealthBar(transform, maxHealth);
    }

    private void Destroy(AttackType attackType)
    {
        enemyBehaviour.enabled = false;
        ColliderGameObject.enabled = false;
        animator.SetTrigger(deathHash);
        healthBarRennder.DestroyHealthBar();
        gameManager.EnemyDeath();
    }

}
