using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class RouteEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Node node, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }

        Gizmos.DrawSphere(node.transform.position, .2f);

        if (node.nextNode != null)
        {
            Route parrent = node.GetComponentInParent(typeof(Route)) as Route;
            if (parrent != null)
                Gizmos.color = parrent.routeColor;

            Gizmos.DrawLine(node.transform.position, node.nextNode.transform.position);
        }
    }
}
