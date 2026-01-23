using System.Collections.Generic;
using UnityEngine;

public class RaycastPerception : Perception
{

    [SerializeField, Tooltip("The number of rays casted.")] int numRays = 1;

    public override GameObject[] GetGameObjects()
    {
        // create result list
        List<GameObject> result = new List<GameObject>();

        // get array of directions in circle
        Vector3[] directions = Utilities.GetDirectionsInCircle(numRays, fov);

        // iterate through directions
        foreach (var direction in directions)
        {
            // create ray from transform postion in the direction of (transform.rotation * direction)
            Ray ray = new Ray(transform.position, transform.rotation * direction);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, layerMask))
            {
                // do not include ourselves
                if (raycastHit.collider.gameObject == gameObject) continue;
                // check for matching tag
                if (tagName == "" || raycastHit.collider.CompareTag(tagName))
                {
                    // add game object to results
                    result.Add(raycastHit.collider.gameObject);
                    Debug.DrawRay(ray.origin, ray.direction * raycastHit.distance, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            }
        }

        // convert list to array
        return result.ToArray();
    }
}
