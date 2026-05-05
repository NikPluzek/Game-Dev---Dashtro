using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AITarget : MonoBehaviour
{

    public Transform Target;
    public float AttackDistance;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private float m_Distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Distance = Vector3.Distance(m_Agent.transform.position, Target.position);
        if (m_Distance < AttackDistance)
        {
            m_Agent.isStopped = true;
            m_Animator.SetBool("Attack", true);
        }
        else
        {
            m_Agent.isStopped = false;
            m_Animator.SetBool("Attack", false);
            m_Agent.destination = Target.position;
        }
    }

    void OnAnimatorMove()
    {
        if (m_Animator.GetBool("Attack") == false)
        {
            {
                m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
            }
        }
    }
}
