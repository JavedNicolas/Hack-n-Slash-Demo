using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Basic Attack", menuName = "Skills/Attacks/Basic Attack")]
public class BasicAttack : Skill
{
    public float damage;

    // attack speed and cd
    float lastTimeUsed = Time.time;

    public BasicAttack(float damage)
    {
        this.damage = damage;
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void effect(Being target, float aspd)
    {
        if(Time.time >= lastTimeUsed + (1.0f/ aspd))
        {
            DamageController.instance.applyDamage(target, damage);
            lastTimeUsed = Time.time;
        }
    }
}
