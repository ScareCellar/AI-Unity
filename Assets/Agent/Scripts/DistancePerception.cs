using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
    // method overrides Perception base class 
    public override GameObject[] GetGameObjects()
    {
        // dynamic list to store perceived game objects 
        List<GameObject> result = new List<GameObject>();

        // get all colliders inside sphere 
        var colliders = Physics.OverlapSphere(transform.position, maxDistance);
        foreach (var collider in colliders)
        {
            // do not include ourselves 
            if (collider.gameObject == gameObject) continue;
            // check for matching tag 
            if (tagName == "" || collider.tag == tagName)
            {
                // check if within max angle range 
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle <= fov)
                {
                    // add game object to result 
                    result.Add(collider.gameObject);
                }
            }
        }

        // sort by distance (nearest first)
        result.Sort((a, b) =>
        {
            // get distance between transform position and a/b position
            float distA = (transform.position = a.transform.position).sqrMagnitude;
            float distB = (transform.position = b.transform.position).sqrMagnitude;

            return distA.CompareTo(distB);
        });

        return result.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;
        
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}