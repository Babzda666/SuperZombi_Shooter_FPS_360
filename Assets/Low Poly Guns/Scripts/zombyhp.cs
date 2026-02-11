using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class zombyhp : MonoBehaviour
{
    public float hp = 100f;
    private bool isDead = false;
    private float MaxHealth = 500f;
    // Посилання на компоненти, які треба вимкнути
    private Animator animator;
    private NavMeshAgent agent;
    private Collider zombieCollider;
    public bool isBoss = false;
    private GameObject BossHealthBar;
    private void Start()
    {
        MaxHealth = hp;
        if (isBoss)
        {
            GameObject.Find("Boss Health Bar").transform.GetChild(0).gameObject.SetActive(true);
            BossHealthBar = GameObject.Find("BossHealthBar");
            BossHealthBar.transform.parent.gameObject.SetActive(true);
        }
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        zombieCollider = GetComponent<Collider>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Якщо вже мертвий, шкоду не отримуємо

        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }

        if (isBoss)
        {
               BossHealthBar.transform.localScale = new Vector3(hp / MaxHealth, 1, 1);
        }
     
    }

    private void Die()
    {
        isDead = true;

        GetComponent<ZombiWalkAi>().enabled = false;
        
        // 1. Анімація смерті
        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // 2. Зупиняємо рух
        if (agent != null)
        {
            agent.isStopped = true; // Зупиняємо навігацію
            agent.enabled = false;   // Вимикаємо компонент зовсім
        }

        // 3. Вимикаємо колізії (щоб гравець не впирався в труп)
        if (zombieCollider != null)
        {
            zombieCollider.enabled = false;
        }

        // 4. Видаляємо об'єкт через 3 секунди
        Destroy(gameObject, 3f);
    }

    // Публічний метод для перевірки стану з інших скриптів
    public bool IsDead()
    {
        return isDead;
    }
}

