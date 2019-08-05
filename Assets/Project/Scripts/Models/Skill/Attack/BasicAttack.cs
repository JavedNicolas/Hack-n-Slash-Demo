using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Basic Attack", menuName = "Skills/Attacks/Basic Attack")]
public class BasicAttack : Skill
{
    public float damage;
    public override SkillType skillType => SkillType.Regular;
    public override SkillCoolDownType coolDownType => SkillCoolDownType.Attack;

    public BasicAttack(float damage)
    {
        this.damage = damage;
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
