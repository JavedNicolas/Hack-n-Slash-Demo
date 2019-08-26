using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Self Heal Effect", fileName = "self Heal Effect")]
public class SelfHealEffect : Effect
{
    public override void effect(BeingBehavior sender, GameObject target, float value)
    {
        sender.being.heal(value);
    }
}
