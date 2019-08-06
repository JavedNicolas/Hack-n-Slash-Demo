using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : ProjectileSkill
{
    public override int numberOfProjectile => 3;
    public override ProjectileFormType projectileFormType => ProjectileFormType.Cone;
    public override float offsetBetweenProjectile => 3;
    public override SkillType skillType => SkillType.Projectile;
    public override SkillCoolDownType coolDownType => SkillCoolDownType.Spell;

    public LightningBall()
    {
        this.model = Resources.Load<GameObject>(SkillConstant.projectilePrefabFolder + "LightningBall");
    }


    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void effect(Being target, Being sender)
    {
        target.takeDamage(50);
    }
}
