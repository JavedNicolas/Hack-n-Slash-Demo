using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Ability
{
    /*public LightningBall() : base(Resources.Load<LightningBallAttributs>(AbilityConstant.attributsFolder + "LightningBallAttributs"))
    {
    }*/

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {

    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition, BeingBehavior senderGameObject)
    {
        LightningBallAttributs attributs = (LightningBallAttributs)_abilityAttributs;

        useEffect(EffectStartingTime.AbilityStart, sender, null);

        AbilityManager.instance.launchProjectile(attributs.ballGameObject, attributs.numberOfBaseProjectile, attributs.offsetBetweenProjectile, attributs.projectileBaseSpeed, attributs.projectileLife, targetedPosition,
            attributs.formType, senderGameObject, _abilityAttributs.effectAndValues);
    }
}
