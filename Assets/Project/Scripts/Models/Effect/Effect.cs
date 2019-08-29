using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [Header("Effect attributs")]
    [SerializeField] EffectStartingTime _startingType;
    public EffectStartingTime startingType { get { return _startingType; } }

    [SerializeField] EffectTargetType _effectTargetType;
    public EffectTargetType effectTargetType { get { return _effectTargetType; } }

    /// <summary>
    /// Apply the effect
    /// </summary>
    /// <param name="sender">The seffect sender</param>
    /// <param name="target">The effect target game object</param>
    /// <param name="value">The effect value</param>
    public abstract void use(BeingBehavior sender, GameObject target, float value);

    /// <summary>
    /// Check if the effect can be used
    /// </summary>
    /// <returns>Return true if the effect can be used</returns>
    public abstract bool canBeUsed(BeingBehavior sender, GameObject target, float value);

    protected bool isCorrectTarget(BeingBehavior sender, GameObject target)
    {
        BeingBehavior targetScript = target.GetComponent<BeingBehavior>();

        switch (effectTargetType)
        {
            case EffectTargetType.Self:
                if (targetScript.being == sender.being)
                    return true;
                break;
            case EffectTargetType.Enemy:
                if (targetScript.teamID != sender.teamID)
                    return true;
                break;
            case EffectTargetType.Allies:
                if (targetScript.teamID == sender.teamID)
                    return true;
                break;
            case EffectTargetType.SelfAndAllies:
                if (targetScript.teamID == sender.teamID || targetScript.being == sender.being)
                    return true;
                break;
            case EffectTargetType.AllBeing:
                if (targetScript.teamID == sender.teamID || targetScript.teamID != sender.teamID || targetScript.being == sender.being)
                    return true;
                break;
        }

        return false;
    }
}
