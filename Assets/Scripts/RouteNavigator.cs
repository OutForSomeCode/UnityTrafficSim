using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteNavigator : MonoBehaviour
{
    public Node current;
    private TrafficManager manager;
    private VehicleController controller;

    private void Awake()
    {
        controller = GetComponent<VehicleController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller.SetDestination(current.GetNodePosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination && current.nextNode != null)
        {
            current = current.nextNode;
            controller.SetDestination(current.GetNodePosition());
        }
    }
}
