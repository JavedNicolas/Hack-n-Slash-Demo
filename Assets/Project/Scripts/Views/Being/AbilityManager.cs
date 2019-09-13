using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeingBehavior))]
public class AbilityManager : MonoBehaviour
{
    [Header("Field of view")]
    static protected LayerMask _blockingElements;

    protected BeingBehavior _abilitySender;
    public BeingBehavior abilitySender { get => _abilitySender; }

    private void Awake()
    {
        _abilitySender = GetComponent<BeingBehavior>();
        _blockingElements = LayerMask.GetMask(LayersIndex.Environment.ToString());
    }

    #region ability usage
    /// <summary>
    /// Try to perform the ability
    /// </summary>
    /// <param name="targetBehavior">The targeted beingBehavior script</param>
    /// <param name="targetedPosition">Mouse position</param>
    /// <param name="ability">The ability</param>
    /// <param name="senderBehavior">The sender BeingBehavior</param>
    /// <returns></returns>
    public bool tryToPerformAbility(BeingBehavior targetBehavior, Vector3 targetedPosition, Ability ability)
    {
        if (ability.isAbilityAvailable(abilitySender.being))
        {
            if(isInRange(ability, targetBehavior == null ? targetedPosition : targetBehavior.transform.position))
                if (ability.abilityAttributs.needTarget)
                {
                    return tryPerformTargetedAbility(targetBehavior, ability);
                }
                else
                {
                    return tryPerformNotTargetedAbility(targetedPosition, ability);
                }
        }

        return false;
    }

    /// <summary>
    /// check if the target or targeted position is in range and not blocked
    /// </summary>
    /// <returns></returns>
    bool isInRange(Ability ability, Vector3 positionToCheck)
    {
        if (!ability.abilityAttributs.checkForRange)
            return true;

        Vector3 senderPosition = abilitySender.transform.position;
        if(Vector3.Distance(positionToCheck, senderPosition) > ability.abilityAttributs.range)
            return false;

        if(Physics.Linecast(senderPosition, positionToCheck, _blockingElements))
            return false;

        return true;
    }

    /// <summary>
    /// Try to perform a targeted ability
    /// </summary>
    /// <param name="targetedTransform"></param>
    /// <param name="ability"></param>
    /// <returns>return false if the ability could not be used, else return true</returns>
    protected virtual bool tryPerformTargetedAbility(BeingBehavior targetedBehavior, Ability ability)
    {
        if (targetedBehavior != null)
        {
            bool canBeUsed = false;
            if (targetedBehavior.being == abilitySender.being && ability.abilityAttributs.canBeCastedOnSelf)
                canBeUsed = true;
            else if (targetedBehavior.being != abilitySender.being)
                canBeUsed = true;

            if (canBeUsed && abilitySender.being is Player)
                canBeUsed = checkMana(ability);

            if (canBeUsed)
            {
                ability.performAbility(abilitySender, targetedBehavior);
                if (abilitySender.being is Player)
                    ((Player)abilitySender.being).spendMana(ability.abilityAttributs.manaCost);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// try to perform a not targeted ability
    /// </summary>
    /// <param name="targetedPosition"></param>
    /// <param name="ability"></param>
    /// <returns>return true if the ability could be used</returns>
    protected virtual bool tryPerformNotTargetedAbility(Vector3 targetedPosition, Ability ability)
    {
        ability.performAbility(abilitySender, targetedPosition);
        return true;
    }

    public bool checkMana(Ability ability)
    {
        Player player = (Player)abilitySender.being;
        if (player != null)
        {
            if (player.currentMana >= ability.abilityAttributs.manaCost)
                return true;
            else
                return false;
        }
        return true;
    }
    #endregion

    #region spawn abilities
    /// <summary>
    /// Spawn an area for an ability
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="areaEffectAndValues">The effect the area will do on collision</param>
    /// <param name="targetedPosition"></param>
    public void spawnArea(Ability ability, List<AbilityEffectAndValue> areaEffectAndValues, Vector3 targetedPosition)
    {
        IAreaAttributs areaAttributs = (IAreaAttributs)ability.abilityAttributs;
        GameObject area = Instantiate(areaAttributs.areaPrefab);
        area.transform.position = targetedPosition;

        if(area.GetComponent<Area>() != null)
        {
            area.GetComponent<Area>().setArea(ability, areaEffectAndValues, _abilitySender);
        }
    }

    /// <summary>
    /// Send projectile for an ability
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="projectileEffectValues">The effect the projectiles do make on collision</param>
    /// <param name="targetedPosition"></param>
    public void launchProjectile(Ability ability, List<AbilityEffectAndValue> projectileEffectValues, Vector3 targetedPosition)
    {
        IProjectileAttributs projectileAttributs = (IProjectileAttributs)ability.abilityAttributs;
        int numberOfProjectile = abilitySender.being.stats.getBuffedValue(1, StatType.NumberOfProjectile, ability.getName(), ability.stats.statList);
        float projectileSpeed = abilitySender.being.stats.getBuffedValue(projectileAttributs.baseProjectileSpeed, StatType.ProjectileSpeed, ability.getName(), ability.stats.statList);

        for (int i = 0; i < numberOfProjectile; i++)
        {
            // create projectile
            GameObject projectile = Instantiate(projectileAttributs.projectilePrefab);
            projectile.transform.position = projectileStartPosition(projectileAttributs.formType, projectileAttributs.offsetBetweenProjectile, i, numberOfProjectile);

            // get projectile direction
            Vector3 direction = projectileDirection(projectileAttributs.formType, projectileAttributs.offsetBetweenProjectile, i, numberOfProjectile, targetedPosition);
            direction.y = 0;

            // Apply force to it
            if (projectile.GetComponent<Rigidbody>() != null)
            {
                projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);
            }
            // set is collision effect
            if (projectile.GetComponent<Projectile>() != null)
            {
                projectile.GetComponent<Projectile>().setProjectile(abilitySender, ability, projectileEffectValues);
            }
            Destroy(projectile, projectileAttributs.projectileLife);
        }
    }

    /// <summary>
    /// Set the projectile starting position
    /// </summary>
    /// <param name="projectile">The projectile Object</param>
    /// <param name="skill">The skill sending the projectile</param>
    /// <param name="projectileCurrentIndex">The current projectile begin sent</param>
    /// <param name="numberOfProjectile">The number of project</param>
    Vector3 projectileStartPosition(ProjectileFormType formType, float projectileOffset, int projectileCurrentIndex, int numberOfProjectile)
    {
        Vector3 position = new Vector3();

        switch (formType) {
            case ProjectileFormType.Line:
                float totalProjectileLineLength = projectileOffset * (numberOfProjectile - 1);
                float currentProjectileOffset = -totalProjectileLineLength / 2 + projectileOffset * projectileCurrentIndex;
                position = transform.position + (transform.right * currentProjectileOffset);
                break;
            case ProjectileFormType.Cone:
                position = transform.position;
                break;
        }

        return position;
    }

    /// <summary>
    /// Set the projectile Direction
    /// </summary>
    /// <param name="skill">The skill sending the projectile</param>
    /// <param name="projectileCurrentIndex">The current projectile being send</param>
    /// <param name="numberOfProjectile"> The total number of projectile</param>
    /// <param name="targetPosition"> The position where the projectile need to head to</param>
    /// <returns></returns>
    Vector3 projectileDirection(ProjectileFormType formType, float projectileOffset, int projectileCurrentIndex, int numberOfProjectile, Vector3 targetPosition)
    {
        Vector3 direction = new Vector3();

        switch (formType)
        {
            case ProjectileFormType.Cone:
                float totalProjectileLineLength = projectileOffset * (numberOfProjectile - 1);
                float currentProjectileOffset = -totalProjectileLineLength / 2 + projectileOffset * projectileCurrentIndex;

                targetPosition = targetPosition + (transform.right * currentProjectileOffset);
                direction = (targetPosition - transform.position).normalized;

                break;
            case ProjectileFormType.Line: direction = (targetPosition - transform.position).normalized; break;
        }
        direction.y = 0;

        return direction;
    }
    #endregion
}
