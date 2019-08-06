using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : ProjectileSkill
{
    public float damage = 10f; 

    public LightningBall(float damage, GameObject projectilePrefab)
    {
        this.damage = damage;
        this.model = projectilePrefab;
        this.skillType = SkillType.Projectile;
        this.coolDownType = SkillCoolDownType.Spell;
        this.projectileFormType = ProjectileFormType.Cone;
        this.numberOfProjectile = 5;
        this.offsetBetweenProjectile = 3;
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
