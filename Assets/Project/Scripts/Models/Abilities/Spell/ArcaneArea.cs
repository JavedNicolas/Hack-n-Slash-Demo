using UnityEngine;
using System.Collections;

public class ArcaneArea : Ability
{
    public ArcaneArea() { }

    public ArcaneArea(ArcaneArea arcaneArea) : base(arcaneArea){}

    public override void setBaseStats()
    {
        
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        sender.abilityManager.spawnArea(this, getEffectFor(EffectType.Area), targetedPosition);
    }
}
