using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Ability : DatabaseElement, IDescribable
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
    abstract public void performAbility(BeingBehavior sender, BeingBehavior targetGameObject);

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    abstract public void performAbility(BeingBehavior sender, Vector3 targedPosition);

    /// <summary>
    /// launch and effect based on his starting time type (used for effect which start on precise movement which we controle, like before the skill or after)
    /// </summary>
    /// <param name="startingType">The starting type</param>
    /// <param name="sender">The sender of the ability</param>
    /// <param name="target">The target of the ability</param>
    protected void useEffect(EffectStartingTime startingType, BeingBehavior sender, GameObject target)
    {
        List<EffectAndValue> effects = _abilityAttributs.effectAndValues;
        for (int i =0; i < effects.Count; i++)
        {
            if (effects[i].effect.startingType == startingType)
                effects[i].effect.use(sender, target, _abilityAttributs.effectAndValues[i].value);
        }
    }

    public bool isAbilityAvailable(Being sender)
    {
        switch (_abilityAttributs.coolDownType)
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

    public string getName()
    {
        return GetType().ToString();
    }

    public virtual string getDescription()
    {
        string description = "";
        for (int i = 0; i < abilityAttributs.effectAndValues.Count; i++)
        {
            description += abilityAttributs.effectAndValues[i].getDescription() + "\n";
        }
        return description;
    }
}
