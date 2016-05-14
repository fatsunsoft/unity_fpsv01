using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
    public float fieldOfView = 90f;
    public bool entityInSight = false;
    public Vector3 personalLastSighting;

    private NavMeshAgent pathfinder;
    private SphereCollider col;
    private LastPlayerSighting lastPlayerSighting;
    private LivingEntity targetEntity;
    private Vector3 previousSighting;

    void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        previousSighting = lastPlayerSighting.resetPosition;
    }

    void Update()
    {
        //if the global sighting of the player has changed
        if(lastPlayerSighting.position != previousSighting)
        {
            personalLastSighting = lastPlayerSighting.position;
        }

        previousSighting = lastPlayerSighting.position;
    }

    void OnTriggerStay(Collider other)
    {
        //if the player (targetEntity) has entered the sphere
        if (other.gameObject == targetEntity)
        {
            //create a vector from enemy to target, store angle between it and forward
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            //if within field of view
            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if(hit.collider.gameObject == targetEntity)
                    {
                        lastPlayerSighting.position = targetEntity.transform.position;
                    }
                }
                if (CalculatePathLength(targetEntity.transform.position) <= col.radius)
                    // ... set the last personal sighting of the player to the player's current position.
                    personalLastSighting = targetEntity.transform.position;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == targetEntity)
        {
            lastPlayerSighting.position = personalLastSighting;
        }
    }

    float CalculatePathLength (Vector3 targetPosition)
    {
        //create a path and set it to target position
        NavMeshPath path = new NavMeshPath();
        if (pathfinder.enabled)
        {
            pathfinder.CalculatePath(targetPosition, path);
        }
        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}