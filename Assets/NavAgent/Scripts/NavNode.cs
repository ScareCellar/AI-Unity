using UnityEngine;

public class NavNode : MonoBehaviour
{
    [SerializeField] protected NavNode[] neighbors;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (NavNode n in neighbors)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
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
