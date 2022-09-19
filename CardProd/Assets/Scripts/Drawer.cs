using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    
}
public class Drawer : MonoBehaviour
{
    private Vector3 m_size = new Vector3(7, 0.1f, 10);
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position,m_size);
    }
}
