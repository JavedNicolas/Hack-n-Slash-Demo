using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability
{
    // attributs
    protected float lastTimeUsed = 0f;
    protected AbilityAttributs abilityAttributs;

    // functions
    public Ability(AbilityAttributs abilityAttributs)
    {
        this.abilityAttributs = abilityAttributs;
    }

    /// <summary>
    /// Start the sender animation for this ability
    /// </summary>
    abstract public void animation();

    /// <summary>
    /// Perform Ability (used for targeted attack)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targetGameObject">The target gameObject</param>
    abstract public void performAbility(BeingBehavior sender, BeingBehavior targetGameObject);

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    abstract public void performAbility(BeingBehavior sender, Vector3 targedPosition, BeingBehavior senderGameObject);

    /// <summary>
    /// launch and effect based on his starting time type (used for effect which start on precise movement which we controle, like before the skill or after)
    /// </summary>
    /// <param name="startingType">The starting type</param>
    /// <param name="sender">The sender of the ability</param>
    /// <param name="target">The target of the ability</param>
    protected void useEffect(EffectStartingTime startingType, BeingBehavior sender, GameObject target)
    {
        List<Effect> effects = abilityAttributs.effectAndValues.effects;
        for (int i =0; i < effects.Count; i++)
        {
            if (effects[i].startingType == startingType)
                effects[i].effect(sender, target, abilityAttributs.effectAndValues.effectValues[i]);
        }
    }

    public bool isAbilityAvailable(Being sender)
    {
        switch (abilityAttributs.coolDownType)
        {
            case AbilityCoolDownType.ASPD: return Time.time >= lastTimeUsed + (1f / sender.getASPD()) ? true : false; 
            case AbilityCoolDownType.Cooldown: break;
            case AbilityCoolDownType.CastSpeed: return Time.time >= lastTimeUsed + (1f / sender.getCastPerSecond()) ? true: false;
        }

        return false;
    }

    /// <summary>
    /// retrived the icon of the ability
    /// </summary>
    /// <returns>A sprite of the icon</returns>
    public Sprite getIcon()
    {
        return abilityAttributs.icon;
    }

    /// <summary>
    /// Get the ability Attributs
    /// </summary>
    /// <returns></returns>
    public AbilityAttributs getAttributs()
    {
        return abilityAttributs;
    }

    /// <summary>
    /// Sert the ability as used and thus start the cooldown (or apsd/ castspeed cooldown)
    /// </summary>
    public void abilityHasBeenUsed()
    {
        lastTimeUsed = Time.time;
    }
}
