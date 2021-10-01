using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node nextNode, previousNode;
    
    public Vector3 GetNodePosition()
    {
        return transform.position;
    }
}
