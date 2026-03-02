using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class WASDMove : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        Animator animator;

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
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 input = new Vector3(h, 0f, v);

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

            if (animator != null)
                animator.SetBool("Running", input.sqrMagnitude > 0.01f);
        }
    }
}