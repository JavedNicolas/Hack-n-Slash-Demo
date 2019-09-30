using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [Header("Effect attributs")]
    [SerializeField] string _description;
    public string description { get { return _description; } }

    /// <summary>
    /// Apply the effect
    /// </summary>
    /// <param name="sender">The seffect sender</param>
    /// <param name="target">The effect target game object</param>
    /// <param name="value">The effect value</param>
    public abstract void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null);

    /// <summary>
    /// Check if the effect can be used
    /// </summary>
    /// <returns>Return true if the effect can be used</returns>
    public abstract bool canBeUsed(BeingBehavior sender, GameObject target, float value);

}
