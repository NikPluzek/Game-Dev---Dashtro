using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class BossEnemy : MonoBehaviour
{
    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 2f;

    [Header("Charge Settings")]
    [SerializeField] private float telegraphDuration = 1.2f;
    [SerializeField] private float chargeSpeed = 18f;
    [SerializeField] private float chargeDistance = 12f;
    [SerializeField] private float chargeCooldown = 4f;
    [SerializeField] private float chargeDamage = 30f;

    [Header("Stun Settings")]
    [SerializeField] private float stunDuration = 2f;

    [Header("Melee Settings")]
    [SerializeField] private float meleeDamage = 15f;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float meleeCooldown = 1.5f;
    private enum BossState { Chasing, Telegraphing, Charging, Stunned }
    private BossState currentState = BossState.Chasing;

    private float chargeTimer = 0f;    
    private float stateTimer = 0f; 
    private float meleeTimer = 0f;

    private Rigidbody rb;

    private bool isActive = false;

    private Vector3 chargeDirection;
    private float chargeDistanceTravelled = 0f;
    private Vector3 chargeStartPosition;

    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth;
    private Renderer bossRenderer;
    private Color originalColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health bossHealth = GetComponent<Health>();
        if (bossHealth != null)
            bossHealth.onDeath.AddListener(OnBossDied);

        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        agent.enabled = false;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();
        }

        bossRenderer = GetComponentInChildren<Renderer>();
        if (bossRenderer != null)
            originalColor = bossRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || player == null) return;

        if (player == null) return;

        if (currentState == BossState.Chasing)
            chargeTimer += Time.deltaTime;

        if (meleeTimer > 0)
            meleeTimer -= Time.deltaTime;

        switch (currentState)
        {
            case BossState.Chasing:
                UpdateChase();
                break;
            case BossState.Telegraphing:
                UpdateTelegraph();
                break;
            case BossState.Charging:
                UpdateCharge();
                break;
            case BossState.Stunned:
                UpdateStun();
                break;
        }
    }


    private void UpdateChase()
    {
        agent.SetDestination(player.position);

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer <= meleeRange && meleeTimer <= 0)
        {
            playerHealth?.TakeDamage(meleeDamage);
            meleeTimer = meleeCooldown;
        }

        if (chargeTimer >= chargeCooldown)
        {
            chargeTimer = 0f;
            EnterTelegraph();
        }
    }

    private void UpdateTelegraph()
    {
        stateTimer += Time.deltaTime;

        if (bossRenderer != null)
        {
            float flash = Mathf.PingPong(stateTimer * 6f, 1f);
            bossRenderer.material.color = Color.Lerp(originalColor, Color.red, flash);
        }

        if (stateTimer >= telegraphDuration)
        {
            EnterCharge();
        }
    }

    private void UpdateCharge()
    {
        float step = chargeSpeed * Time.deltaTime;
        chargeDistanceTravelled += step;

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < 1.5f)
        {
            playerHealth?.TakeDamage(chargeDamage);
            EnterChase();
            return;
        }

        if (chargeDistanceTravelled >= chargeDistance)
        {
            EnterStun();
        }
    }

    private void UpdateStun()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= stunDuration)
        {
            EnterChase();
        }
    }

    private void EnterTelegraph()
    {
        currentState = BossState.Telegraphing;
        stateTimer = 0f;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        chargeDirection = (player.position - transform.position).normalized;
        chargeDirection.y = 0f;

        transform.rotation = Quaternion.LookRotation(chargeDirection);
    }

    private void EnterCharge()
    {
        currentState = BossState.Charging;
        chargeDistanceTravelled = 0f;
        chargeStartPosition = transform.position;
        agent.enabled = false;

        if (bossRenderer != null)
            bossRenderer.material.color = originalColor;
    }

    private void EnterStun()
    {
        currentState = BossState.Stunned;
        stateTimer = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (bossRenderer != null)
            bossRenderer.material.color = Color.blue;
    }

    private void EnterChase()
    {
        currentState = BossState.Chasing;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (!agent.enabled)
            agent.enabled = true;
        if (bossRenderer != null)
            bossRenderer.material.color = originalColor;
    }

    public void Activate()
    {
        isActive = true;
        agent.enabled = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currentState == BossState.Charging)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                EnterStun();
            }
        }
    }

    void FixedUpdate()
    {
        if (!isActive || currentState != BossState.Charging) return;

        Debug.Log("Charging! Distance travelled: " + chargeDistanceTravelled);
        rb.linearVelocity = chargeDirection * chargeSpeed;
    }

    private void OnBossDied()
    {
        GameManager.Instance?.TriggerWin();
    }

}
