using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node nextNode, previousNode;
    public float allowedSpeed;
    
    public Vector3 GetNodePosition()
    {
        return transform.position;
    }

    public float GetAllowedSpeed()
    {
        return allowedSpeed;
    }

    public float GetTurnAngle()
    {
        if (nextNode != null)
        {
            Vector3 targetDir = nextNode.transform.position - transform.position;
            Vector3 forwardDir = previousNode != null ? transform.position - previousNode.transform.position : transform.forward;
            float angle = Vector3.Angle(targetDir, forwardDir);

            return angle;
        }
        
        return 0;
    }
}
