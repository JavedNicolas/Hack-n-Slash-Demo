using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArcaneArea : Ability
{
    public override void setBaseStats()
    {
        
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        sender.abilityManager.spawnArea(this, getEffectFor(EffectUseBy.Area), targetedPosition);
    }

    public override void animation()
    {
       
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior target)
    {
        throw new System.NotImplementedException();
    }
}
