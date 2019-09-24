using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArcaneArea : Ability
{
    public ArcaneArea() : base() { }

    public ArcaneArea(ArcaneArea arcaneArea) : base(arcaneArea){}

    public override void setBaseStats()
    {
        
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        sender.abilityManager.spawnArea(this, getEffectFor(EffectType.Area), targetedPosition);
    }

    public override void animation()
    {
       
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {
        throw new System.NotImplementedException();
    }
}
