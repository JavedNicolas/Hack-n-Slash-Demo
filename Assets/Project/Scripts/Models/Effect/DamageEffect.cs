using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Damage Effect", fileName = "Damage Effect")]
public class DamageEffect : Effect
{
    public override void effect(BeingBehavior sender, GameObject target, float value)
    {
        BeingBehavior targetScript = target.GetComponent<BeingBehavior>();
        if(targetScript != null && isCorrectTarget(sender, target))
        {
            targetScript.being.takeDamage(value);
        }
    }
}
