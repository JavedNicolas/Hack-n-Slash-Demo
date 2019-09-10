using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ability
{
    public LightningBall() {}

    public LightningBall(LightningBall lightningBall)
    {
        this._abilityAttributs = lightningBall.abilityAttributs;
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        sender.abilityManager.launchProjectile(this, getEffectFor(EffectType.Projectile), targetedPosition);
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {
        throw new NotImplementedException();
    }
}
