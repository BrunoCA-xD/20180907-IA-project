using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacersBehavior : MonoBehaviour {

    public float m_MaxSpeed;
    public float m_MaxForce;
    public float m_SlowingRadius;
    public PathFollow m_Path;
    private Vector3 m_CurrentVelocity;
    

    // Use this for initialization
    private void Start()
    {
        m_CurrentVelocity = transform.forward * m_MaxSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

    }

    private Vector3 Seek(Vector3 target, float slowingRadius)
    {
        Vector3 desiredVelocity = target - transform.position;

        float distance = desiredVelocity.magnitude;
        float speed = m_MaxSpeed * Time.fixedDeltaTime;
        float ratio = distance / slowingRadius;

        desiredVelocity = (distance < slowingRadius) 
            ? desiredVelocity * ratio * speed  
            : desiredVelocity.normalized * m_MaxSpeed * Time.fixedDeltaTime;

        Vector3 steeringForce = desiredVelocity - m_CurrentVelocity;
        steeringForce = steeringForce.normalized * m_MaxForce * Time.fixedDeltaTime;

        return steeringForce;
    }

    public Vector3 PathFollow()
    {
        Vector3? target = m_Path.GetNode();
        if (Vector3.Distance(transform.position, (Vector3)target) <= m_Path.m_Radius)
            m_Path.NextNode();

        return Seek((Vector3)target, m_SlowingRadius);
    }
}
