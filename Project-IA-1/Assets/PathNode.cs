using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public Color m_PointColor;
    public Color m_RandiusColor = Color.white;

    public float m_Radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = m_PointColor;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = m_RandiusColor;
        Gizmos.DrawSphere(transform.position, m_Radius);
    }   
}
