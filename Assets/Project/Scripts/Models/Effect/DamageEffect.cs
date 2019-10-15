using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : Effect
{
    protected override string description => "Damage_Description".localize();

    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        BeingBehavior targetBehavior = target.GetComponent<BeingBehavior>();
        if (targetBehavior != null)
            return true;

        return false;
    }

    public override void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null)
    {
        if(canBeUsed(sender, target, value))
        {
            BeingBehavior targetScript = target.GetComponent<BeingBehavior>();
            targetScript.takeDamage(value, effectOrigin, sender);
        }
    }
}
