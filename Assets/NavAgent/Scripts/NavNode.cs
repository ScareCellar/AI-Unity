using UnityEngine;
using System.Collections.Generic;
public class NavNode : MonoBehaviour
{
    [SerializeField] protected List<NavNode> neighbors;

    public List<NavNode> Neighbors { get { return neighbors; }  set { neighbors = value; } }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<NavAgent>(out NavAgent navAgent))
        {
            navAgent.OnEnterNavNode(this);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (NavNode n in neighbors)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }

    public static NavNode GetNearestNavNode(Vector3 position)
    {   
        NavNode nearest = null;
        float nearestDistance = float.MaxValue;

        var navNodes = GetAllNavNodes();
        foreach (NavNode n in navNodes)
        {
            float distance = Vector3.Distance(n.transform.position, position);
            if (distance < nearestDistance)
            {
                nearest = n;
                nearestDistance = distance;
            }
        }
        return nearest;
    }

    #region helper_functions

    public static NavNode[] GetAllNavNodes()
    {
        return FindObjectsByType<NavNode>(FindObjectsSortMode.None);
    }

    public static NavNode GetRandomNavNode()
    {
        var nodes = GetAllNavNodes();

        return (nodes.Length == 0) ? null : nodes[Random.Range(0, nodes.Length)];
    }

    #endregion
}
