using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    private List<Transform> routes;
    // Start is called before the first frame update
    void Start()
    {
        routes = new List<Transform>();
        Transform root = GameObject.Find("Routes").transform;

        for (int i = 0; i < root.childCount; i++)
        {
            routes.Add(root.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetRoute()
    {
        return routes[0];
    }
}
