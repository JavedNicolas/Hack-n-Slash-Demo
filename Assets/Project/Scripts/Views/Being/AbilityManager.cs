﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeingBehavior))]
public class AbilityManager : MonoBehaviour
{
    BeingBehavior abilitySender;

    private void Awake()
    {
        abilitySender = GetComponent<BeingBehavior>();
    }

    /// <summary>
    /// Try to perform the ability (used for player)
    /// </summary>
    /// <param name="mouseHit">Mouse position</param>
    /// <param name="ability">The ability</param>
    /// <param name="senderBehavior">The sender BeingBehavior</param>
    /// <returns></returns>
    public bool tryToPerformAbility(RaycastHit mouseHit, Ability ability)
    {
        if (ability.isAbilityAvailable(abilitySender.being))
        {
            if(ability.abilityAttributs.needTarget)
            {
                BeingBehavior targetScript = mouseHit.transform.GetComponent<BeingBehavior>();
                if (targetScript != null)
                {
                    bool canBeUsed = false;
                    if (targetScript.being == abilitySender.being && ability.abilityAttributs.canBeCastedOnSelf)
                        canBeUsed = true;

                    if(canBeUsed)
                        canBeUsed = checkMana(ability);

                    if (canBeUsed)
                    {
                        ability.performAbility(abilitySender, targetScript);
                        if (abilitySender.being is Player)
                            ((Player)abilitySender.being).spendMana(ability.abilityAttributs.manaCost);
                        return true;
                    }
                }
            }
            else
            {
                if (checkMana(ability))
                {
                    ability.performAbility(abilitySender, mouseHit.point);
                    if (abilitySender.being is Player)
                        ((Player)abilitySender.being).spendMana(ability.abilityAttributs.manaCost);
                    return true;
                }
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Try to perform the ability (used for AI)
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
            if (ability.abilityAttributs.needTarget)
            {
                if (targetBehavior != null)
                {
                    bool canBeUsed = false;
                    if (targetBehavior.being == abilitySender.being && ability.abilityAttributs.canBeCastedOnSelf)
                        canBeUsed = true;
                    else if (targetBehavior.being != abilitySender.being)
                        canBeUsed = true;

                    if (canBeUsed)
                    {
                        ability.performAbility(abilitySender, targetBehavior);
                        return true;
                    }
                }
            }
            else
            {
                ability.performAbility(abilitySender, targetedPosition);
                return true;
            }
        }

        return false;
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

    public void launchProjectile(GameObject projectilePrefab, int numberOfProjectile, float projectileOffset, float projectileBaseSpeed, float projectileLife, 
        Vector3 targetedPosition, ProjectileFormType formType, List<EffectAndValue> effectAndValues)
    {
        for (int i = 0; i < numberOfProjectile; i++)
        {
            // create projectile
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileStartPosition(formType, projectileOffset, i, numberOfProjectile);

            // get projectile direction
            Vector3 direction = projectileDirection(formType, projectileOffset, i, numberOfProjectile, targetedPosition);
            direction.y = 0;

            // Apply force to it
            if (projectile.GetComponent<Rigidbody>() != null)
            {
                float projectileSpeed = projectileBaseSpeed + (projectileBaseSpeed * (abilitySender.being.projectileSpeed / 100));
                projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);
            }


            // set is collision effect
            if (projectile.GetComponent<Projectile>() != null)
            {
                List<EffectAndValue> effectAndValuesToUse = new List<EffectAndValue>();
                for (int e = 0; e < effectAndValues.Count; e++)
                    if (effectAndValues[e].effect.startingType == EffectStartingTime.Hit)
                    {
                        effectAndValuesToUse.Add(new EffectAndValue());
                        effectAndValuesToUse[e].addEffectAndValue(effectAndValues[e].effect, effectAndValues[e].value);
                    }
                        

                projectile.GetComponent<Projectile>().setProjectile(abilitySender, effectAndValuesToUse);
            }

            Destroy(projectile, projectileLife);
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
}