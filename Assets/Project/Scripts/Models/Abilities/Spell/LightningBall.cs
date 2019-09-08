using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ability
{
    LightningBallAttributs _attributs;

    public LightningBall() { }

    public LightningBall(LightningBall lightningBall)
    {
        this._abilityAttributs = lightningBall.abilityAttributs;
        _attributs = (LightningBallAttributs)_abilityAttributs;
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        _attributs.sendProjectile(sender, targetedPosition, GetType().Name);
    }
}
