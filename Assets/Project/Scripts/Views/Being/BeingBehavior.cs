using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BeingBehavior : MonoBehaviour
{
    public Transform transformToMove;
    public Being being;

    [Header("Speed")]
    public float gameObjectHeight;
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
    /// Launch the skill effect based on his type
    /// </summary>
    /// <param name="skill"> The skill to launch</param>
    /// <param name="targetedPosition"> The targeted position (can be an enemy, the position of the mouse or else).</param>
    /// <param name="target"> The target of the spell if there is one, null if not</param>
    protected void attack(Skill skill, Vector3 targetedPosition, BeingBehavior target = null)
    {
        lookAtStraight(targetedPosition);
        switch (skill.skillType)
        {
            case SkillType.Self: break;
            case SkillType.InFront: break;
            case SkillType.SelfArea: break;
            case SkillType.MousePosition: break;
            case SkillType.Projectile: projectile((ProjectileSkill)skill, targetedPosition); break;
            case SkillType.Regular: attackRegular(target, skill); break;
            case SkillType.line: break;
            
        }
    }

    void attackRegular(BeingBehavior target, Skill skill)
    {
        if(target != null && skill.isSkillAvailable(being))
        {
            skill.effect(target.being, being);
            skill.skillHasBeenUsed();
        }
            
    }

    void projectile(ProjectileSkill skill, Vector3 targetPosition)
    {
        stopMoving();

        if (skill.isSkillAvailable(being))
        {
            // create projectile
            GameObject projectile = Instantiate(skill.model);
            projectile.transform.position = transform.position;

            // get projectile direction
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            // Apply force to it
            if (projectile.GetComponent<Rigidbody>() != null)
                projectile.GetComponent<Rigidbody>().AddForce(direction * 100, ForceMode.Impulse);

            // set is collision effect
            if (projectile.GetComponent<Projectile>() != null)
            {
                projectile.GetComponent<Projectile>().sender = being;
                projectile.GetComponent<Projectile>().projectileCollisionDelegate = skill.effect;
            }
            skill.skillHasBeenUsed();

            Destroy(projectile, 1f);
        }
       
        
    }

    protected void die()
    {
        Destroy(gameObject);
    }
}
