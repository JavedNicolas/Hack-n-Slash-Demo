using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : ProjectileSkill
{
    public float damage = 10f; 

    public override SkillType skillType => SkillType.Projectile;
    public override SkillCoolDownType coolDownType => SkillCoolDownType.Spell;

    public LightningBall(float damage, GameObject projectilePrefab)
    {
        this.damage = damage;
        this.model = projectilePrefab;
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void effect(Being target, Being sender)
    {
        target.takeDamage(damage);
    }
}
