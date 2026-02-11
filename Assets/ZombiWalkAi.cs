
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombiWalkAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Animator animator;
    private zombyhp healthScript; // Посилання на скрипт здоров'я

    [Header("Speeds (units/sec)")]
    public float speedWalking = 1.5f;
    public float speedRunning = 4.0f;
    public float speedAttacking = 1.0f;

    [Header("Timing / Ranges")]
    public float walkDuration = 5f;
    public float attackRange = 2f;
    public float attackDuration = 1.2f;

    private enum State { Walking, Running, Attacking }
    private State currentState = State.Walking;
    private State previousState = State.Walking;
    private float walkTimer = 0f;
    private Coroutine attackCoroutine;

    public float ZombieAttackdamage = 7;
    
    private readonly int hashIsWalking = Animator.StringToHash("isWalking");
    private readonly int hashIsRunning = Animator.StringToHash("isRunning");
    private readonly int hashAttack = Animator.StringToHash("Attack");

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Знаходимо скрипт здоров'я на цьому ж об'єкті
        healthScript = GetComponent<zombyhp>();

        currentState = State.Walking;
        previousState = State.Walking;
        ApplyAnimatorParamsForState(currentState, force: true);
        agent.speed = speedWalking;
    }

    private void Update()
    {
        // ПЕРЕВІРКА НА СМЕРТЬ: якщо мертвий — нічого не робимо
        if (healthScript != null && healthScript.IsDead()) return;

        if (player == null || agent == null) return;

        agent.SetDestination(player.position);
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange && currentState != State.Attacking)
        {
            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            attackCoroutine = StartCoroutine(AttackRoutine());
            return;
        }

        if (currentState == State.Attacking) return;

        if (currentState == State.Walking)
        {
            walkTimer += Time.deltaTime;
            if (walkTimer >= walkDuration)
            {
                ChangeState(State.Running);
            }
        }
    }

    private void ChangeState(State newState)
    {
        // Якщо мертвий, стани не змінюємо
        if (healthScript != null && healthScript.IsDead()) return;
        
        if (currentState == newState) return;
        previousState = currentState;
        currentState = newState;

        switch (newState)
        {
            case State.Walking:
                agent.speed = speedWalking;
                walkTimer = 0f;
                ApplyAnimatorParamsForState(State.Walking);
                break;

            case State.Running:
                agent.speed = speedRunning;
                ApplyAnimatorParamsForState(State.Running);
                break;

            case State.Attacking:
                agent.speed = speedAttacking;
                ApplyAnimatorParamsForState(State.Attacking);
                break;
        }
    }

    private void ApplyAnimatorParamsForState(State state, bool force = false)
    {
        if (animator == null) return;
        if (!force && previousState == state) return;

        if (state == State.Walking)
        {
            animator.SetBool(hashIsWalking, true);
            animator.SetBool(hashIsRunning, false);
        }
        else if (state == State.Running)
        {
            animator.SetBool(hashIsWalking, false);
            animator.SetBool(hashIsRunning, true);
        }
    }

    private IEnumerator AttackRoutine()
    {
        ChangeState(State.Attacking);


        // Перевіряємо чи зомбі ще живий в момент нанесення шкоди
        if (healthScript != null && !healthScript.IsDead())
        {
            var playerHP = FindFirstObjectByType<PlayerHeals>();
            if (playerHP != null) playerHP.TakeDamage(ZombieAttackdamage);
            
            if (animator != null)
            {
                animator.SetTrigger(hashAttack);
            }
        }

        yield return new WaitForSeconds(attackDuration);

        // Після паузи повертаємось до ходьби, тільки якщо не померли за цей час
        if (healthScript != null && !healthScript.IsDead())
        {
            ChangeState(State.Walking);
        }

        attackCoroutine = null;
    }
}

