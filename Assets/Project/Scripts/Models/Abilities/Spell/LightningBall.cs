﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightningBall : Ability
{
    public override void setBaseStats()
    {
        stats.addStat(new Stat(StatType.NumberOfProjectile, StatBonusType.Pure, 1, AbilityConstant.abilityBaseSourceName, StatInfluencedBy.Intelligence, 10, getName()));
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        sender.abilityManager.launchProjectile(this, getEffectFor(EffectUseBy.Projectile), targetedPosition);
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior target)
    {
        throw new NotImplementedException();
    }
}
