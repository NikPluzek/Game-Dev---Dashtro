using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackCooldownRemaining = 0f;
    private Transform player;
    private Health playerHealth;


    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();
        }
        else
        {
            Debug.LogError("EnemyAttack: No GameObject Found with tag 'Player'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldownRemaining > 0)
        {
            attackCooldownRemaining -= Time.deltaTime;
        }

        if (player != null && attackCooldownRemaining <= 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        attackCooldownRemaining = attackCooldown;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Enemy attacked player. Player health: " + playerHealth.GetCurrentHealth());
        }

    }
}
