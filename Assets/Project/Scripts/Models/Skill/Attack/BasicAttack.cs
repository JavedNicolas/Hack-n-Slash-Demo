using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill
{
    public override SkillType skillType => SkillType.Regular;
    public override SkillCoolDownType coolDownType => SkillCoolDownType.Attack;
    public override Sprite icon => throw new System.NotImplementedException();

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void effect(Being target, Being sender)
    {
       target.takeDamage(10);
        Debug.Log(sender.name + " " + Time.time);
    }
}
