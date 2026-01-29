using UnityEngine;

public class NavAgent : AIAgent
{
    [SerializeField] Movement movement;

    public NavNode TargetNode {  get; set; }
    void Start()
    {
        TargetNode = NavNode.GetNearestNavNode(transform.position);
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
            TargetNode = navNode.Neighbors[Random.Range(0, navNode.Neighbors.Count)];
        }
    }
}
