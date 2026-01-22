
using UnityEngine;


public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception seekPerception;
    [SerializeField] Perception fleePerception;

    [Header("Wander")]
    [SerializeField] float wanderRadius = 1;
    [SerializeField] float wanderDistance = 1;
    [SerializeField] float wanderDisplacement = 1;
    
    float wanderAngle = 0.0f;

    void Start()
    {
        wanderAngle = Random.Range(0,360);
    }

    void Update()
    {
        bool hasTarget = false;
        if (seekPerception != null)
        {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                hasTarget = true;
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                hasTarget = true;
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            } 
        }

        if (!hasTarget)
        {
            Vector3 force = Wander();
            //< apply force to movement >
            movement.ApplyForce(force);
        }

        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, 0, -15), new Vector3(15, 0, 15));
        
        if (movement.Velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }

    Vector3 Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    Vector3 Flee(GameObject target)
    {
        Vector3 direction = transform.position - target.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        return Vector3.ClampMagnitude(steer, movement.maxForce);
    }
    private Vector3 Wander()
    {

        // randomly adjust the wander angle within (+/-) displacement range 
        wanderAngle += Random.Range(-wanderDisplacement, wanderDisplacement);
        // calculate a point on the wander circle using the wander angle 

        Quaternion rotation = Quaternion.AngleAxis(wanderAngle, Vector3.up);

        Vector3 pointOnCircle = rotation * (Vector3.forward * wanderRadius);

        // project the wander circle in front of the agent 

        Vector3 circleCenter = movement.Direction * wanderDistance;

        // steer toward the target point (circle center + point on circle) 

        Vector3 force = GetSteeringForce(circleCenter + pointOnCircle);



        return force;

    }
}