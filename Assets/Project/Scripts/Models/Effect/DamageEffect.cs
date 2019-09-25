using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Damage Effect", fileName = "Damage Effect")]
public class DamageEffect : Effect
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
        if(canBeUsed(sender, target, value))
        {
            BeingBehavior targetScript = target.GetComponent<BeingBehavior>();
            targetScript.takeDamage(value, effectOrigin, sender);
        }
    }
}
