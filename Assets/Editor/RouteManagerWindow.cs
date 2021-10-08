using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RouteManagerWindow : EditorWindow
{
    public string[] routeNames;

    private float maxSpeed;
    private List<Transform> routes;
    private Transform nodeRoot;
    private int routeCount;
    private int index = 0;

    [MenuItem("Tools/Node Editor")]
    public static void Open()
    {
        GetWindow<RouteManagerWindow>();
    }

    private void OnGUI()
    {
        int count = GameObject.Find("Routes").transform.childCount;
        SerializedObject obj = new SerializedObject(this);

        //EditorGUILayout.PropertyField(obj.FindProperty("routes"));

        if (count != routeCount)
        {
            routeCount = count;
            routes = new List<Transform>();
            routeNames = new string[count];

            for (int i = 0; i < count; i++)
            {
                routes.Add(GameObject.Find("Routes").transform.GetChild(i));
                routeNames[i] = GameObject.Find("Routes").transform.GetChild(i).name;
            }
        }

        if (routeNames.Length != 0)
        {
            index = EditorGUILayout.Popup(index, routeNames);
            nodeRoot = routes[index];
        }

         maxSpeed = EditorGUILayout.Slider("Max speed", 50, 0, 130);

        if (routes.Count == 0)
        {
            EditorGUILayout.HelpBox("please create a route", MessageType.Warning);
        }

        if (nodeRoot == null)
        {
            EditorGUILayout.HelpBox("please select a route", MessageType.Warning);
        }

        EditorGUILayout.BeginVertical("Box");
        DrawButtons();
        EditorGUILayout.EndVertical();
        

        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Route"))
            CreateRoute();

        if (GUILayout.Button("Create Node"))
            CreateNode();
    }

    private void CreateRoute()
    {
        Transform routesRoot = GameObject.Find("Routes").transform;
        GameObject routeObj = new GameObject("Route " + routesRoot.childCount, typeof(Route));
        routeObj.transform.SetParent(routesRoot, false);
    }

    private void CreateNode()
    {
        GameObject nodeObj = new GameObject("Node " + nodeRoot.childCount, typeof(Node));
        nodeObj.transform.SetParent(nodeRoot, false);

        Node node = nodeObj.GetComponent<Node>();
        node.allowedSpeed = maxSpeed;

        if (nodeRoot.childCount > 1)
        {
            node.previousNode = nodeRoot.GetChild(nodeRoot.childCount - 2).GetComponent<Node>();
            node.previousNode.nextNode = node;
        }
        Selection.activeGameObject = node.gameObject;
    }
}
