using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Heal Effect", fileName = "Heal Effect")]
public class HealEffect : Effect
{
    public override void effect(BeingBehavior sender, GameObject target, float value)
    {
        BeingBehavior targetScript = target.GetComponent<BeingBehavior>();
        if (targetScript != null && isCorrectTarget(sender, target))
        {
            sender.being.heal(value);
        }
    }
}
