using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BeingBehavior : InteractableObject
{
    [Header("Being model")]
    public NavMeshAgent navMeshAgent;

    [Header("Navigation")]
    [SerializeField] private float gameObjectHeight;
    [SerializeField] private float slowingDistance = 1;

    [Header("Team")]
    public bool canBeAttacked = true;
    public int teamID;

    // Path variable
    protected List<Vector3> _path = new List<Vector3>();
    protected NavMeshPath _navMeshPath;
    protected Vector3 _yOffset;

    // Mesh attributs
    [SerializeField] private Animator _animator;
    private MeshFilter _meshFilter;

    //Ability manager
    protected AbilityManager _abilityManager;
    public AbilityManager abilityManager { get => _abilityManager; }

    // being
    protected Being _being;
    public Being being
    {
        get { return _being; }
        set {
            _being = value;
            interactable = value;
        }

    }

    // distance stop
    protected float _distanceToStop = 0.1f;

    // Interaction
    protected InteractableObject _interactionTarget;// interactible current target
    protected bool _interactOnce = false;

    void Awake()
    {
        if (navMeshAgent != null)
        {
            _animator = navMeshAgent.gameObject.GetComponent<Animator>();
            if (_animator == null)
                _animator = navMeshAgent.gameObject.GetComponentInChildren<Animator>();
        }

        _abilityManager = GetComponent<AbilityManager>();
        _navMeshPath = new NavMeshPath();
        setMeshYOffset();
    }

    void FixedUpdate()
    {
        clampCurrentLife();
        updateBuffs();
        interactWithCurrentInteractionTarget();
        playRunAnimation();
    }

    void setMeshYOffset()
    {
        _yOffset = new Vector3(0, 0, 0);
    }

    // play the run animation smoothly
    void playRunAnimation()
    {
        if (_animator == null)
            return;

        if (navMeshAgent.velocity.magnitude == 0)
            _animator.SetFloat("SpeedPercent", 0);
        else
            _animator.SetFloat("SpeedPercent", 1);

    }

    void clampCurrentLife()
    {
        being.clampLife();
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

    #region movement
    /// <summary>
    /// Create a path to the destination
    /// </summary>
    /// <param name="destination">The destination to reach</param>
    /// <param name="distanceToStop">Distance from which the move need to be stopped</param>
    public void moveTo(Vector3 destination, float distanceToStop = 0.1f)
    {
        this._distanceToStop = distanceToStop;

        navMeshAgent.stoppingDistance = this._distanceToStop;
        navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Check if the player is moving toward a position
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>True if the player is moving toward this position, else false</returns>
    public bool isMovingToward(Vector3 position)
    {
        if (_path.Count == 0)
            return false;

        if (_path[_path.Count - 1].x == position.x && _path[_path.Count - 1].z == position.z)
            return true;

        return false;
    }

    protected void stopMoving()
    {
        _path.Clear();
    }
    #endregion

    #region ability
    /// <summary>
    /// Use the being ability
    /// </summary>
    /// <param name="ability">ability to use</param>
    /// <param name="targetedPosition">the targeted position for the ability (use for ability which do not need target but target point)</param>
    /// <param name="target">The targeted being behavior</param>
    public bool useAbility(Ability ability, Vector3 targetedPosition, BeingBehavior target)
    {
        if(abilityManager.tryToPerformAbility(target, targetedPosition, ability))
        {
            abilityUsed(targetedPosition, target, ability);
            return true;
        }
        return false;
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
    #endregion

    #region buffs
    void updateBuffs()
    {
        for(int i= 0; i < being.buffs.Count; i++)
            if(being.buffs[i].duration != -1)
                if (Time.time >= being.buffs[i].startingTime + being.buffs[i].duration)
                    being.removeBuff(being.buffs[i].name);
    }
    #endregion

    #region interaction

    /// <summary>
    /// Add a interactable target for being to interact with
    /// </summary>
    /// <param name="interactableObject">the interactable target</param>
    public void AddInteractableTaget(InteractableObject interactableObject)
    {
        this._interactionTarget = interactableObject;
    }
    /// <summary>
    /// Move the player to the interactible object
    /// </summary>
    /// <param name="fromRightClick">If the call comes from the right mouse click</param>
    /// <returns>return true if the interaction is possible</returns>
    protected bool moveToInteractableTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<InteractableObject>() != null)
        {
            InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();

            _interactionTarget = interactableObject;
            return true;
        }
        return false;
    }


    /// <summary>
    /// Move to the interactable object and interact with it when in range
    /// </summary>
    void interactWithCurrentInteractionTarget()
    {
        if (_interactionTarget != null)
        {
            float interactionDistance = _interactionTarget.interactable.getInteractionDistance();
            if (_interactionTarget.interactable.interactibleType == InteractableObjectType.Enemy)
                interactionDistance = being.stats.attackRange;

            if (Vector3.Distance(transform.position, _interactionTarget.transform.position) <= interactionDistance)
            {
                lookAtStraight(_interactionTarget.transform.position);
                if (_interactionTarget.interactable.interact(this, _interactionTarget.gameObject))
                    _interactionTarget = null;
                stopMoving();
            }
            else
            {
                moveTo(_interactionTarget.transform.position, interactionDistance);
            }
        }
    }
    #endregion

    /// <summary>
    /// Deal damage to this being
    /// </summary>
    /// <param name="damage">The damage to deal</param>
    /// <param name="damageDealer">The damage dealer</param>
    public virtual void takeDamage(float damage, DatabaseElement damageOrigin, BeingBehavior damageDealer)
    {
        being.takeDamage(damage);
        if (being.isDead())
        {
            if (damageDealer is PlayerBehavior)
            {
                PlayerBehavior playerBehavior = (PlayerBehavior)damageDealer;
                Enemy enemy = (Enemy)being;
                playerBehavior.addExperience(enemy.experience);
            }
            
            if(!(damageDealer is PlayerBehavior))
                die();
        }
    }

    protected virtual bool isBeingInRange(BeingBehavior beingBehavior, float distance)
    {
        return Vector3.Distance(beingBehavior.transform.position, transform.position) <= distance;
    }

    protected virtual void die()
    {
        Destroy(gameObject);
    }
}
