using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Effect
{
    protected override string description => "Heal_Description".localize();

    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        if (sender.being.currentLife != sender.being.stats.maxLife)
            return true;
        return false; 
    }

    public override void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null)
    {
        if(canBeUsed(sender, target, value))
            sender.being.heal(value);
    }
}
