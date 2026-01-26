
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception seekPerception;
    [SerializeField] Perception fleePerception; 

    [Header("Wander")]
    [SerializeField] float wanderRadius = 1;
    [SerializeField] float wanderDistance = 1;
    [SerializeField] float wanderDisplacement = 1;

    [Header("Flock")]
    [SerializeField] Perception flockPerception;
    [SerializeField, Range(0, 5)] float cohesionWeight = 1;
    [SerializeField, Range(0, 5)] float separationWeight = 1;
    [SerializeField, Range(0, 5)] float alignmentWeight = 1;
    [SerializeField, Range(0, 5)] float separationRadius = 1;

    [Header("Obstacle")]
    [SerializeField] Perception obstaclePerception;
    [SerializeField, Range(0, 5)] float obstacleWeight = 1;

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

        if (flockPerception != null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                hasTarget = true;
                movement.ApplyForce(Cohesion(gameObjects) * cohesionWeight);
                movement.ApplyForce(Separation(gameObjects, separationRadius) * separationWeight);
                movement.ApplyForce(Alignment(gameObjects) * alignmentWeight);
            }
        }

        if (obstaclePerception != null && obstaclePerception.GetGameObjectInDirection(transform.forward) != null)
        {
            Vector3 openDirection = Vector3.zero;
            if (obstaclePerception.GetOpenDirection(ref openDirection))
            {
                hasTarget = true;
                movement.ApplyForce(GetSteeringForce(openDirection * obstacleWeight));
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

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        // accumulate the position vectors of the neighbors
        foreach (var neighbor in neighbors)
        {
            // add neighbor position to positions
            positions += neighbor.transform.position;
        }

        // average the positions to get the center of the neighbors
        Vector3 center = positions / neighbors.Count();
        // create direction vector to point towards the center of the neighbors from agent position
        Vector3 direction = center - transform.position;

        // steer towards the center point
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        // accumulate the separation vectors of the neighbors
        foreach (var neighbor in neighbors)
	    {
            // get direction vector away from neighbor
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = direction.magnitude;
            // check if within separation radius
            if (distance > 0 && distance < radius)
		    {
                // scale separation vector inversely proportional to the direction distance
                // closer the distance the stronger the separation
                separation += direction * (1 / distance);
            }
        }

        // steer towards the separation point
        Vector3 force = (separation.magnitude > 0) ? GetSteeringForce(separation) : Vector3.zero;

        return force;
    }

    private Vector3 Alignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        // accumulate the velocity vectors of the neighbors
        foreach (var neighbor in neighbors)
	    {
            // get the velocity from the agent movement
            if (TryGetComponent<AutonomousAgent>(out AutonomousAgent neighborAA) == true)
		    {
                // add agent movement velocity to velocities
                velocities += neighborAA.movement.Velocity;
            }
        }
        // get the average velocity of the neighbors
        Vector3 averageVelocity = velocities / neighbors.Count();

        // steer towards the average velocity
        Vector3 force = GetSteeringForce(averageVelocity);


    return force;
    }
}