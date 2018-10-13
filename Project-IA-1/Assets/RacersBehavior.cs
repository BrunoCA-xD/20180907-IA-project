using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacersBehavior : MonoBehaviour {

    public float m_MaxSpeed;
    public float m_MaxForce;
    public float m_SlowingRadius;
    public float m_Mass;
    public PathFollow m_Path;
    private Vector3 m_CurrentVelocity;
    [Header("Collision Avoidance")]
    public float m_AvoidanceForce;
    public float m_MaxSeeAhead;
    public LayerMask m_ObstacleLayer;
    public float[] m_RaycastAngles;


    // Use this for initialization
    private void Start()
    {
        m_CurrentVelocity = transform.forward * m_MaxSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 steeringForce = Vector3.zero;
        steeringForce += PathFollow();

        for (int i = 0; i < m_RaycastAngles.Length; i++)
        {
            float angle = m_RaycastAngles[i];
            steeringForce += CollisionAvoidance(angle);
        }

        steeringForce += CollisionAvoidance(0);
        steeringForce += CollisionAvoidance(-20);
        steeringForce += CollisionAvoidance(20);

        Vector3 acceleration = steeringForce / m_Mass;
        float random = Random.Range(0.1f, 1.0f);

        m_CurrentVelocity += acceleration;

        transform.position += m_CurrentVelocity;

        transform.rotation = Quaternion.LookRotation(m_CurrentVelocity); ;
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

    public Vector3 CollisionAvoidance(float angle)
    {
        Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,//origem
            direction,//direção
            out hitInfo,//info da colisao
            m_MaxSeeAhead,//distancia
            m_ObstacleLayer//camada
            ))
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            float ratio = 1 - (hitInfo.distance / m_MaxSeeAhead);
            return hitInfo.normal * (ratio * m_AvoidanceForce);
        }
        else
        {
            Debug.DrawLine(transform.position,
                transform.position + direction * m_MaxSeeAhead,
                Color.blue);
        }
        return Vector3.zero;

    }
}
