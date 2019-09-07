using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ability
{
    public LightningBall() { }

    public LightningBall(LightningBall lightningBall)
    {
        this._abilityAttributs = lightningBall.abilityAttributs;
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {

    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        LightningBallAttributs attributs = (LightningBallAttributs)_abilityAttributs;

        useEffect(EffectStartingTime.AbilityStart, sender, null);

        Being senderBeing = sender.being;

        sender.abilityManager.launchProjectile(attributs.ballGameObject, senderBeing.getBuffedValue(attributs.numberOfBaseProjectile, StatType.NumberOfProjectile), 
            attributs.offsetBetweenProjectile, senderBeing.getBuffedValue(attributs.projectileBaseSpeed, StatType.ProjectileSpeed), attributs.projectileLife, targetedPosition,
            attributs.formType, _abilityAttributs.effectAndValues);
    }

    public override string getDescription(Being owner)
    {
        string description = base.getDescription(owner);
        // cast the attributs
        ProjectileAbilityAttributs projectileAbilityAttributs = (ProjectileAbilityAttributs)abilityAttributs;

        // get the buffed value of projectile Number
        int numberOfProjectile = owner.getBuffedValue(projectileAbilityAttributs.numberOfBaseProjectile, StatType.NumberOfProjectile);

        // insert at the start of the description the number of projectile
        string lightningBallDescription = abilityAttributs.description.Replace("{0}", numberOfProjectile.ToString()) + "\n";

        return description.Insert(0, lightningBallDescription);
    }
}
