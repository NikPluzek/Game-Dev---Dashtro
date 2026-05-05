using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer; // Which layer enemies are on

    private float attackCooldownRemaining = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldownRemaining > 0)
        {
            attackCooldownRemaining -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && attackCooldownRemaining <= 0)
        {
            Attack();
            Debug.Log($"mouse down");
        }
    }

    private void Attack()
    {
        attackCooldownRemaining = attackCooldown;

        // Find all colliders within attack range
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            attackRange,
            enemyLayer
        );

        Debug.Log("Enemies found in range: " + hitColliders.Length);

        foreach (Collider hit in hitColliders)
        {
            // Check the enemy is within the swing arc (in front of us)
            Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            Debug.Log(hit.gameObject.name + " | Angle: " + angleToEnemy + " | Max allowed: " + (attackAngle / 2f));

            if (angleToEnemy <= attackAngle / 2f)
            {
                // Enemy is within the arc - deal damage
                Health enemyHealth = hit.GetComponent<Health>();

                Debug.Log("Health component found: " + (enemyHealth != null));

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void IncreaseDamage(float amount)
    {
        attackDamage += amount;
    }

    public void IncreaseRange(float amount)
    {
        attackRange += amount;
    }

    //visual indicator for hitbox
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
