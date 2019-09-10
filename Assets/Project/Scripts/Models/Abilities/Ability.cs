using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class Ability : DatabaseElement
{
    // attributs
    // handle the cooldown
    protected float lastTimeUsed = 0f;

    // Level
    [SerializeField] protected int _currentLevel;
    public int currentLevel { get => _currentLevel == 0 ? 1 : _currentLevel; }

    // Ability attributs
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
    abstract public void performAbility(BeingBehavior sender, BeingBehavior targetGameObject);

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    abstract public void performAbility(BeingBehavior sender, Vector3 targedPosition);

    /// <summary>
    /// Use every effect for the effectType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="forEffectType"></param>
    public void useEffects(BeingBehavior sender, GameObject target, EffectType forEffectType)
    {
        foreach(AbilityEffectAndValue effectAndValue in abilityAttributs.effectAndValues)
        {
            if(effectAndValue.effectType == forEffectType)
                effectAndValue.useEffect(sender, target, this);
        }
    }

    /// <summary>
    /// Get all the effect a given effect Type
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public List<AbilityEffectAndValue> getEffectFor(EffectType effectType)
    {
        return abilityAttributs.effectAndValues.FindAll(x => x.effectType == effectType).ToList();
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

    /// <summary>
    /// Increase the level of the ability if the level isn't maxed
    /// </summary>
    /// <returns></returns>
    public bool levelUp()
    {
        if (currentLevel == abilityAttributs.maxLevel - 1)
            return false;

        _currentLevel++;
        return true;
    }
}
