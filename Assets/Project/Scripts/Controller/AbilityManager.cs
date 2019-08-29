using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Try to perform the ability (used for player)
    /// </summary>
    /// <param name="mouseHit">Mouse position</param>
    /// <param name="ability">The ability</param>
    /// <param name="senderScript">The sender BeingBehavior</param>
    /// <returns></returns>
    public bool tryToPerformAbility(RaycastHit mouseHit, Ability ability, BeingBehavior senderScript)
    {
        if (ability.isAbilityAvailable(senderScript.being))
        {
            if(ability.abilityAttributs.needTarget)
            {
                BeingBehavior targetScript = mouseHit.transform.GetComponent<BeingBehavior>();
                if (targetScript != null)
                {
                    bool canBeUsed = false;
                    if (targetScript.being == senderScript.being && ability.abilityAttributs.canBeCastedOnSelf)
                        canBeUsed = true;

                    if (canBeUsed)
                    {
                        ability.performAbility(senderScript, targetScript);
                        return true;
                    }
                }
            }
            else
            {
                ability.performAbility(senderScript, mouseHit.point, senderScript);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Try to perform the ability (used for AI)
    /// </summary>
    /// <param name="targetScript">The targeted beingBehavior script</param>
    /// <param name="targetedPosition">Mouse position</param>
    /// <param name="ability">The ability</param>
    /// <param name="senderScript">The sender BeingBehavior</param>
    /// <returns></returns>
    public bool tryToPerformAbility(BeingBehavior targetScript, Vector3 targetedPosition, Ability ability, BeingBehavior senderScript)
    {
        if (ability.isAbilityAvailable(senderScript.being))
        {
            if (ability.abilityAttributs.needTarget)
            {
                if (targetScript != null)
                {
                    bool canBeUsed = false;
                    if (targetScript.being == senderScript.being && ability.abilityAttributs.canBeCastedOnSelf)
                        canBeUsed = true;
                    else if (targetScript.being != senderScript.being)
                        canBeUsed = true;

                    if (canBeUsed)
                    {
                        ability.performAbility(senderScript, targetScript);
                        return true;
                    }
                }
            }
            else
            {
                ability.performAbility(senderScript, targetedPosition, senderScript);
                return true;
            }
        }

        return false;
    }

    public void launchProjectile(GameObject projectilePrefab, int numberOfProjectile, float projectileOffset, float projectileBaseSpeed, float projectileLife, 
        Vector3 targetedPosition, ProjectileFormType formType, BeingBehavior sender, List<EffectAndValue> effectAndValues)
    {
        for (int i = 0; i < numberOfProjectile; i++)
        {
            // create projectile
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileStartPosition(formType, projectileOffset, i, numberOfProjectile, sender.transform);

            // get projectile direction
            Vector3 direction = projectileDirection(formType, projectileOffset, i, numberOfProjectile, targetedPosition, sender.transform);
            direction.y = 0;

            // Apply force to it
            if (projectile.GetComponent<Rigidbody>() != null)
            {
                float projectileSpeed = projectileBaseSpeed + (projectileBaseSpeed * (sender.being.projectileSpeed / 100));
                projectile.GetComponent<Rigidbody>().AddForce(direction * projectileSpeed, ForceMode.Impulse);
            }


            // set is collision effect
            if (projectile.GetComponent<Projectile>() != null)
            {
                List<EffectAndValue> effectAndValuesToUse = new List<EffectAndValue>();
                for (int e = 0; e < effectAndValues.Count; e++)
                    if (effectAndValues[e].effect.startingType == EffectStartingTime.Hit)
                        effectAndValuesToUse[e].addEffectAndValue(effectAndValues[e].effect, effectAndValues[e].value);

                projectile.GetComponent<Projectile>().setProjectile(sender, effectAndValuesToUse);
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
    Vector3 projectileStartPosition(ProjectileFormType formType, float projectileOffset, int projectileCurrentIndex, int numberOfProjectile, Transform sender)
    {
        Vector3 position = new Vector3();

        switch (formType) {
            case ProjectileFormType.Line:
                float totalProjectileLineLength = projectileOffset * (numberOfProjectile - 1);
                float currentProjectileOffset = -totalProjectileLineLength / 2 + projectileOffset * projectileCurrentIndex;
                position = sender.position + (sender.right * currentProjectileOffset);
                break;
            case ProjectileFormType.Cone:
                position = sender.position;
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
    Vector3 projectileDirection(ProjectileFormType formType, float projectileOffset, int projectileCurrentIndex, int numberOfProjectile, Vector3 targetPosition, Transform sender)
    {
        Vector3 direction = new Vector3();

        switch (formType)
        {
            case ProjectileFormType.Cone:
                float totalProjectileLineLength = projectileOffset * (numberOfProjectile - 1);
                float currentProjectileOffset = -totalProjectileLineLength / 2 + projectileOffset * projectileCurrentIndex;

                targetPosition = targetPosition + (sender.right * currentProjectileOffset);
                direction = (targetPosition - sender.position).normalized;

                break;
            case ProjectileFormType.Line: direction = (targetPosition - sender.position).normalized; break;
        }
        direction.y = 0;

        return direction;
    }
}
