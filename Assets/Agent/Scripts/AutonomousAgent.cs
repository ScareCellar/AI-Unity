using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception perception;

    void Start()
    {

    }

    void Update()
    {
        movement.ApplyForce(Vector3.forward);

        transform.position = Utilities.Wrap(transform.position, new Vector3 (-15,-15,-15), new Vector3(15,15,15));
    }
}