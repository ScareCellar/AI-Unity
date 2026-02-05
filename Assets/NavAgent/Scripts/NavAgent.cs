using UnityEngine;

public class NavAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] NavPath path;
    public NavNode TargetNode {  get; set; }
    void Start()
    {
        TargetNode = NavNode.GetNearestNavNode(transform.position);
        if(path != null)
        {
            path.GeneratePath(TargetNode.transform.position, TargetNode.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetNode != null)
        {
            Vector3 direction = TargetNode.transform.position - transform.position;
            Vector3 force = direction.normalized * movement.maxForce;

            movement.ApplyForce(force);
        }
        if (movement.Velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Velocity);
        }
    }

    public void OnEnterNavNode(NavNode navNode)
    {

        if (navNode == TargetNode)
        {
            if(path != null)
            {
                TargetNode = path.GetNextNavNode(navNode);
                if (TargetNode == null)
                {
                    TargetNode = path.GeneratePath(navNode, NavNode.GetRandomNavNode());
                }
            }
            TargetNode = navNode.Neighbors[Random.Range(0, navNode.Neighbors.Count)];
        }
    }
}
