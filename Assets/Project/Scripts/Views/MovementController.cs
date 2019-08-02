using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class MovementController : MonoBehaviour
{
    public Transform transformToMove;

    [Header("Speed")]
    public float gameObjectHeight;
    float movementSpeed;
    Vector3 yOffset;


    // Path variable
    private List<Vector3> path = new List<Vector3>();
    NavMeshPath navMeshPath;

    // distance stop
    float distanceToStop = 0.1f;

    void Awake()
    {
        navMeshPath = new NavMeshPath();
        yOffset = new Vector3(0, gameObjectHeight / 2 - 0.1f, 0);
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
        NavMesh.CalculatePath(transformToMove.position, destination, NavMesh.AllAreas, navMeshPath);
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
        Vector3 currentPathDestination = path[0] + yOffset;
        transformToMove.position = Vector3.MoveTowards(transformToMove.position, currentPathDestination, movementSpeed * Time.deltaTime);
        lookAtStraight(currentPathDestination);

        if (transformToMove.position == currentPathDestination ||(path.Count == 1 && Vector3.Distance(transformToMove.position, currentPathDestination) <= distanceToStop))
        {
            path.RemoveAt(0);
        }
    }

    protected void setMoveSpeed(float moveSpeedPercent)
    {
        movementSpeed = BeingConstant.baseMoveSpeed * moveSpeedPercent / 100;
    }

    protected void lookAtStraight(Vector3 position)
    {
        transform.LookAt(position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
