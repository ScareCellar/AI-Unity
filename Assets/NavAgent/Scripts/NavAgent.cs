using UnityEngine;

public class NavAgent : AIAgent
{
    [SerializeField] Movement movement;

    public NavNode TargetNode {  get; set; }
    void Start()
    {
        TargetNode = NavNode.GetRandomNavNode();
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
    }
}
