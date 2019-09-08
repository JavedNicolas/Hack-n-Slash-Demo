using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class Ability : DatabaseElement
{
    // attributs
    protected float lastTimeUsed = 0f;

    [SerializeField] protected AbilityAttributs _abilityAttributs;
    public AbilityAttributs abilityAttributs { get => _abilityAttributs; }


    /// <summary>
    /// Start the sender animation for this ability
    /// </summary>
    abstract public void animation();

    /// <summary>
    /// Perform Ability (used for targeted attack)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targetGameObject">The target gameObject</param>
    virtual public void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {
        useEffect(EffectStartingTime.AbilityStart, sender, targetGameObject.gameObject);
    }

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    virtual public void performAbility(BeingBehavior sender, Vector3 targedPosition)
    {
        useEffect(EffectStartingTime.AbilityStart, sender, null);
    }

    /// <summary>
    /// launch and effect based on his starting time type (used for effect which start on precise movement which we controle, like before the skill or after)
    /// </summary>
    /// <param name="startingType">The starting type</param>
    /// <param name="sender">The sender of the ability</param>
    /// <param name="target">The target of the ability</param>
    protected void useEffect(EffectStartingTime startingType, BeingBehavior sender, GameObject target)
    {
        List<EffectAndValue> effects = _abilityAttributs.effectAndValues;
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].startingTime == startingType)
            {
                effects[i].useEffect(sender, target, this);
            }

        }
    }

    /// <summary>
    /// Check if the ability if available (cooldown type)
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public bool isAbilityAvailable(Being sender)
    {
        switch (_abilityAttributs.coolDownType)
        {
            case AbilityCoolDownType.ASPD: return Time.time >= lastTimeUsed + (1f / sender.stats.attackSpeed) ? true : false;
            case AbilityCoolDownType.Cooldown: break;
            case AbilityCoolDownType.CastSpeed: return Time.time >= lastTimeUsed + (1f / sender.stats.castSpeed) ? true : false;
        }

        return false;
    }

    /// <summary>
    /// retrived the icon of the ability
    /// </summary>
    /// <returns>A sprite of the icon</returns>
    public Sprite getIcon()
    {
        return _abilityAttributs.icon;
    }

    public void setAttributs(AbilityAttributs attributs)
    {
        _abilityAttributs = attributs;
    }

    /// <summary>
    /// Sert the ability as used and thus start the cooldown (or apsd/ castspeed cooldown)
    /// </summary>
    public void abilityHasBeenUsed()
    {
        lastTimeUsed = Time.time;
    }

    public override string getName()
    {
        return GetType().ToString();
    }

    /// <summary>
    /// Return the description based on the effect
    /// </summary>
    /// <param name="owner">The owner of the ability (to get the buffedValues)</param>
    /// <returns></returns>
    public override string getDescription(Being owner)
    {
        return abilityAttributs.description;
    }
}
