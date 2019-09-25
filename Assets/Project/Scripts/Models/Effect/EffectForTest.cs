using UnityEngine;
using System.Collections;

public class EffectForTest : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        BeingBehavior targetBehavior = target.GetComponent<BeingBehavior>();
        if (targetBehavior != null && isCorrectTarget(sender, targetBehavior))
            return true;

        return false;
    }

    public override void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null)
    {

    }
}
