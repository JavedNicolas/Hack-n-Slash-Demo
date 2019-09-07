using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Self Heal Effect", fileName = "self Heal Effect")]
public class SelfHealEffect : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        if (sender.being.currentLife != sender.being.maxLife)
            return true;
        return false; 
    }

    public override void use(BeingBehavior sender, GameObject target, float value,Ability abilitySender = null)
    {
        if(canBeUsed(sender, target, value))
            sender.being.heal(value);
    }
}
