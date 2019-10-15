using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected abstract string description { get; }

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

    public string getName()
    {
        return GetType().ToString();
    }

    public virtual string getDescription(float buffedValue, Element element = Element.None)
    {
        string newDescription = description.Replace("{Element}", DescriptionText.getElementTypeText(element));
        newDescription = newDescription.Replace("{Value}", buffedValue.ToString());
        return newDescription.sentenceFormat();
    }
}
