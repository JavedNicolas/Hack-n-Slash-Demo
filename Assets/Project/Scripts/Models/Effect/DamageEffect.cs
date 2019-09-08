using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Damage Effect", fileName = "Damage Effect")]
public class DamageEffect : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        if (target.GetComponent<BeingBehavior>() != null && isCorrectTarget(sender, target))
            return true;

        return false;
    }

    public override void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null)
    {
        if(canBeUsed(sender, target, value))
        {
            BeingBehavior targetScript = target.GetComponent<BeingBehavior>();
            targetScript.takeDamage(value, effectOrigin, sender);
            Debug.Log(value);
        }
    }
}
