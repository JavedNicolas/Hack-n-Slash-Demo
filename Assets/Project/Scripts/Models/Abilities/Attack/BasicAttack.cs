﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{

    public BasicAttack() : base()
    {
        setAttributs(Resources.Load<BasicAttackAttributs>(AbilityConstant.attributsFolder + "BasicAttackAttributs"));
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior target)
    {
        useEffects(sender, target.gameObject, EffectUseBy.OnUse);
    }

    public override void performAbility(BeingBehavior sender, Vector3 targedPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void setBaseStats()
    {
        
    }
}
