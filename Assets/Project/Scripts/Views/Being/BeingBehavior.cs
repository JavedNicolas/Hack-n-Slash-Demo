using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BeingBehavior : InteractableObject
{
    [Header("Being model")]
    public Transform transformToMove;

    [Header("Speed")]
    public float gameObjectHeight;

    [Header("Team")]
    public int teamID;

    // Path variable
    protected List<Vector3> path = new List<Vector3>();
    protected NavMeshPath navMeshPath;
    protected Vector3 yOffset;

    // being
    private Being _being;
    public Being being
    {
        get { return _being; }
        set {
            _being = value;
            interactable = value;
        }

    }

    // distance stop
    protected float distanceToStop = 0.1f;

    // Interaction
    protected InteractableObject interactionTarget;// interactible current target
    protected bool interactOnce = false;

    void Awake()
    {
        navMeshPath = new NavMeshPath();
        yOffset = new Vector3(0, gameObjectHeight / 2 - 0.1f, 0);
    }

    void FixedUpdate()
    {
        getToNextPathNode();
        interactWithCurrentInteractionTarget();
    }

    /// <summary>
    /// Create a path to the destination
    /// </summary>
    /// <param name="destination">The destination to reach</param>
    /// <param name="distanceToStop">Distance from which the move need to be stopped</param>
    public void moveTo(Vector3 destination, float distanceToStop = 0.1f)
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
        transformToMove.position = Vector3.MoveTowards(transformToMove.position, currentPathDestination, being.getMovementSpeed() * Time.deltaTime);
        lookAtStraight(currentPathDestination);

        if (transformToMove.position == currentPathDestination ||(path.Count == 1 && Vector3.Distance(transformToMove.position, currentPathDestination) <= distanceToStop))
        {
            path.RemoveAt(0);
        }
    }

    protected void stopMoving()
    {
        path.Clear();
    }

    /// <summary>
    /// Look at the position with x and z rotation locked in place
    /// </summary>
    /// <param name="position"></param>
    protected void lookAtStraight(Vector3 position)
    {
        transform.LookAt(position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    /// <summary>
    /// Use the being ability
    /// </summary>
    /// <param name="ability">ability to use</param>
    /// <param name="targetedPosition">the targeted position for the ability (use for ability which do not need target but target point)</param>
    /// <param name="target">The targeted being behavior</param>
    public void useAbility(Ability ability, Vector3 targetedPosition, BeingBehavior target)
    {
        if(AbilityManager.instance.tryToPerformAbility(target, targetedPosition, ability, this))
        {
            abilityUsed(targetedPosition, target, ability);
        }
    }

    /// <summary>
    /// Set the lookAt, stop the sender and set the ability has used when the ability has been used
    /// </summary>
    /// <param name="mouseHit"></param>
    /// <param name="ability"></param>
    protected void abilityUsed(Vector3 position, BeingBehavior target,Ability ability)
    {
        if (ability.abilityAttributs.needTarget)
        {
            lookAtStraight(target.transform.position);
            stopMoving();
            ability.abilityHasBeenUsed();
        }
        else
        {
            lookAtStraight(position);
            stopMoving();
            ability.abilityHasBeenUsed();
        }
    }

    /// <summary>
    /// Move the player to the interactible object
    /// </summary>
    /// <param name="fromRightClick">If the call comes from the right mouse click</param>
    /// <returns>return true if the interaction is possible</returns>
    protected bool moveToInteractibleTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<InteractableObject>() != null)
        {
            InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();

            interactionTarget = interactableObject;
            return true;
        }
        return false;
    }


    /// <summary>
    /// Move to the interactable object and interact with it when in range
    /// </summary>
    void interactWithCurrentInteractionTarget()
    {
        if (interactionTarget != null)
        {
            float interactionDistance = interactionTarget.interactable.getInteractionDistance();
            if (interactionTarget.interactable.interactibleType == InteractableObjectType.Enemy)
                interactionDistance = being.attackRange;

            if (Vector3.Distance(transform.position, interactionTarget.transform.position) <= interactionDistance)
            {
                lookAtStraight(interactionTarget.transform.position);
                if (interactionTarget.interactable.interact(this, interactionTarget.gameObject))
                    interactionTarget = null;
                stopMoving();
            }
            else
            {
                moveTo(interactionTarget.transform.position, interactionDistance);
            }
        }
    }

    /// <summary>
    /// Check if the player is moving toward a position
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>True if the player is moving toward this position, else false</returns>
    public bool isMovingToward(Vector3 position)
    {
        if (path.Count == 0)
            return false;

        if (path[path.Count - 1].x == position.x && path[path.Count - 1].z == position.z)
            return true;

        return false;
    }

    protected virtual void die()
    {
        Destroy(gameObject);
    }
}
