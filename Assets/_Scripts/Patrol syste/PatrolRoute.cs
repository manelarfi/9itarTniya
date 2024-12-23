using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;


public class PatrolRoute : MonoBehaviour
{
    public enum PatrolType
    {
        Loop = 0,
        PingPong = 1,
        Random = 2
    }

    [SerializeField] private Color _patrolRouteColor = Color.green;
    public PatrolType patrolType;
    public List<Transform> route;

    [ProButton]
    private void AddPatrolPoint()
    {
        GameObject thisPoint = new GameObject();
        thisPoint.transform.position = transform.position;
        thisPoint.transform.parent = transform;
        thisPoint.name = "Point" + (route.Count + 1);

#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(thisPoint, "Add Patrol Point");
#endif

        route.Add(thisPoint.transform);

        var leftPoint = Quaternion.AngleAxis(30f, transform.up) * transform.forward;
    }

    [ProButton]
    private void ReversePatrolRoute()
    {
        route.Reverse();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position, gameObject.name);
#endif

        Gizmos.color = _patrolRouteColor;

        for (int i = 0; i < route.Count - 1; i++)
        {
            Gizmos.DrawLine(route[i].position, route[i + 1].position);
        }

        if (patrolType == PatrolType.Loop || patrolType == PatrolType.Random)
        {
            Gizmos.DrawLine(route[route.Count - 1].position, route[0].position);
        }
    }
}