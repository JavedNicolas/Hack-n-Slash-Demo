using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class MovementController : MonoBehaviour
{
    [Header("Speed")]
    public float gameObjectHeight;
    float movementSpeed;


    // Path variable
    private List<Vector3> path = new List<Vector3>();
    NavMeshPath navMeshPath;
    float distanceToStop = 0.1f;

    void Awake()
    {
        navMeshPath = new NavMeshPath();
    }

    void FixedUpdate()
    {
        getToNextPathNode();
    }

    /// <summary>
    /// Create a path to the destination
    /// </summary>
    /// <param name="destination">The destination to reach</param>
    /// <param name="distanceToStop">Distance from which the move need to be stopped</param>
    protected void moveTo(Vector3 destination, float distanceToStop = 0.1f)
    {
        NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath);
        this.distanceToStop = distanceToStop;

        if (navMeshPath.corners.Length > 0)
            path = navMeshPath.corners.ToList();

        getToNextPathNode();
    }

    /// <summary>
    /// Move to the next path point
    /// </summary>
    protected void getToNextPathNode()
    {
        if (path.Count == 0)
            return;

        Vector3 yOffset = new Vector3(0, gameObjectHeight / 2 - 0.1f, 0);
        Vector3 currentPathDestination = path[0] + yOffset;
        transform.position = Vector3.MoveTowards(transform.position, currentPathDestination, movementSpeed * Time.deltaTime);
        transform.LookAt(currentPathDestination);

        if (transform.position == currentPathDestination ||(path.Count == 1 && Vector3.Distance(transform.position, currentPathDestination) <= distanceToStop))
        {
            path.RemoveAt(0);
        }
    }

    protected void setMoveSpeed(float moveSpeedPercent)
    {
        movementSpeed = BeingConstant.baseMoveSpeed * moveSpeedPercent / 100;
    }
}
