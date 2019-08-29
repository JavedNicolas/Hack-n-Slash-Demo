using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Self Heal Effect", fileName = "self Heal Effect")]
public class SelfHealEffect : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        if (sender.being.currentLife != sender.being.getCurrentMaxLife())
            return true;
        return false; 
    }

    public override void use(BeingBehavior sender, GameObject target, float value)
    {
        if(canBeUsed(sender, target, value))
            sender.being.heal(value);
    }
}
