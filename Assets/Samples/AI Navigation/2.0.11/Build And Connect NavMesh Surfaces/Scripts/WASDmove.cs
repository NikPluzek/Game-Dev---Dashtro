using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WASDMove : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        Animator animator;

        [Header("Dash Settings ~")]
        [SerializeField] private float dashSpeed = 15f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 2f;

        private bool isDashing = false;
        private float dashTimeRemaining = 0f;
        private Vector3 dashDirection;
        private float cooldownTimeRemaining = 0f;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            m_Agent.updatePosition = true;
            m_Agent.updateRotation = false;

            if (animator != null)
                animator.applyRootMotion = false;
        }

        void Update()
        {
            if (cooldownTimeRemaining > 0)
            {
                cooldownTimeRemaining -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isDashing && cooldownTimeRemaining <= 0)
            {
                TryStartDash();
            }
            if (isDashing)
            {
                dashTimeRemaining -= Time.deltaTime;
                if (dashTimeRemaining <= 0)
                {
                    EndDash();
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 input = new Vector3(h, 0f, v);

            if (!isDashing)
            {
                if (input.magnitude > 0.1f)
                {
                    Vector3 camForward = Camera.main.transform.forward;
                    Vector3 camRight = Camera.main.transform.right;

                    camForward.y = 0f;
                    camRight.y = 0f;

                    Vector3 moveDir =
                        camForward.normalized * v +
                        camRight.normalized * h;

                    // Drive NavMeshAgent velocity instead of Move()
                    m_Agent.velocity = moveDir.normalized * m_Agent.speed;

                    // Smooth rotation
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        Quaternion.LookRotation(moveDir),
                        720f * Time.deltaTime
                    );
                }
                else
                {
                    m_Agent.velocity = Vector3.zero;
                }
            }
            
            if (animator != null)
                animator.SetBool("Running", input.sqrMagnitude > 0.01f);
        }

        private void TryStartDash()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 input = new Vector3(h, 0f, v);

            if (input.magnitude < 0.1f)
                return;

            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            dashDirection = (camForward.normalized * v + camRight.normalized * h).normalized;

            isDashing = true;
            dashTimeRemaining = dashDuration;
            cooldownTimeRemaining = dashCooldown;

            m_Agent.velocity = dashDirection * dashSpeed;
        }

        private void EndDash()
        {
            isDashing = false;
        }

        public void ResetDashCooldown()
        {
            cooldownTimeRemaining = 0f;
        }

        public bool IsDashReady()
        {
            return cooldownTimeRemaining <= 0 && !isDashing;
        }

        public float GetDashCooldownProgress()
        {
            if (dashCooldown <= 0) return 1f;
            return 1f - Mathf.Clamp01(cooldownTimeRemaining / dashCooldown);
        }

        public void IncreaseDashSpeed(float amount)
        {
            dashSpeed += amount;
            Debug.Log("New dash speed: " + dashSpeed);
        }
    }




}