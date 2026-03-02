using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    /// <summary>
    /// Use physics raycast hit from mouse click to set agent destination
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMove : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        RaycastHit m_HitInfo = new RaycastHit();
        private Animator animator;

        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                    m_Agent.destination = m_HitInfo.point;
            }

            if (m_Agent.velocity.magnitude != 0f)
            {
                animator.SetBool("Running", true);
            }

            else
            {
                animator.SetBool("Running", false);
            }
        }

        void OnAnimatorMove()
        {
            if (animator.GetBool("Running"))
            {
                m_Agent.speed =
                (animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
    }
}