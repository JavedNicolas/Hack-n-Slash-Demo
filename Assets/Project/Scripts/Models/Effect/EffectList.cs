using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EffectList
{
    public static List<Effect> effects = new List<Effect>()
    {
        new DamageEffect(),
        new RecycleEffect(),
        new HealEffect()
    };

    public static string[] getEffectsNames()
    {
        List<string> names = new List<string>();
        foreach(Effect effect in effects)
        {
            names.Add(effect.GetType().ToString());
        }

        return names.ToArray();
    }

    /// <summary>
    /// get the index for an effect
    /// </summary>
    /// <param name="effectName">the effect Name</param>
    /// <returns></returns>
    public static int getIndexfor(string effectName)
    {
        return effects.FindIndex(x => x.getName() == effectName);
    }

    /// <summary>
    /// get the effect a name
    /// </summary>
    /// <param name="effectName">the effect Name</param>
    /// <returns></returns>
    public static Effect getEffectfor(string effectName)
    {
        return effects.Find(x => x.getName() == effectName);
    }

}
