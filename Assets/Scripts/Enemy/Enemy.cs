using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    #region INSTANCE VARIABLES

    public delegate void DeathEvent(Transform enemy);
    public static event DeathEvent OnDeath;

    public event Action OnLoadFinished;

    [Header("References")]
    private CharacterController characterController;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Health")]
    [SerializeField]
    protected float maxHealth = 50f;
    public float currentHealth;
    public float damageTaken;
    public string partHit;

    [Header("Attack")]
    [SerializeField]
    private float damage = 10f;
    public delegate void AttackEvent(IDamageable target);
    public event AttackEvent OnAttack;

    [Header("Stun")]
    [SerializeField]
    private float stunTime;
    public Coroutine stunCoroutine;
    public event Action OnStunFinished;
    public delegate void HitEvent(float amount, string tag);
    public event HitEvent OnHit;

    [Header("Follow")]
    [SerializeField]
    private float updateRate = 0.1f;
    [SerializeField]
    private float attackRadius = 5f;
    [SerializeField]
    private LayerMask playerMask;
    private Transform target;
    private Coroutine followCoroutine;

    [Header("Movement")]
    private Vector3 previousPosition;
    private Vector3 movementVelocity;
    private Vector3 velocity;

    [Header("Jump")]
    [SerializeField]
    private float gravityMultiplier = 10f;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    #endregion

    #region INSTANTIATION

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        previousPosition = characterController.transform.position;
        OnLoadFinished?.Invoke();
    }

    #endregion

    #region HIT
    public void GetAttack(IDamageable target)
    {
        OnAttack?.Invoke(target);
        target.SubtractHealth(damage);
    }

    public void GetHit(float amount, string tag)
    {
        OnHit?.Invoke(amount, tag);
    }

    public void SubtractHealth(float amount)
    {
        currentHealth -= amount;
    }

    public void StartStun()
    {
        stunCoroutine = StartCoroutine(StunDuration());
    }

    public void StopStun()
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
    }

    public virtual IEnumerator StunDuration()
    {
        yield return new WaitForSeconds(stunTime);
        OnStunFinished?.Invoke();
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(transform);
        Destroy(gameObject);
    }

    #endregion

    #region FOLLOW

    public void StartFollow(Transform target)
    {
        this.target = target;
        followCoroutine = StartCoroutine(FollowTarget());
    }

    public void StopFollow()
    {
        agent.SetDestination(transform.position);
        StopCoroutine(followCoroutine);
        target = null;
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (enabled)
        {
            agent.SetDestination(target.position);
            movementVelocity = (characterController.transform.position - previousPosition) / Time.deltaTime;
            animator.SetFloat("Speed", movementVelocity.magnitude);

            previousPosition = characterController.transform.position;

            yield return wait;
        }
    }

    public bool InRange()
    {
        return Physics.CheckSphere(transform.position, attackRadius, playerMask);
    }

    #endregion

    #region MOVEMENT
    public void ApplyGravity(bool grounded)
    {
        if (grounded)
        {
            velocity.y = -2f;
        }

        velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    #endregion
}
