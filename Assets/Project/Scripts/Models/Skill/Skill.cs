using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    protected float lastTimeUsed = 0f;

    public abstract Sprite icon { get; }
    public abstract SkillType skillType { get; }
    public abstract SkillCoolDownType coolDownType { get; }

    abstract public void animation();
    abstract public void effect(Being target, Being sender);

    public bool isSkillAvailable(Being sender)
    {
        switch (coolDownType)
        {
            case SkillCoolDownType.Attack: return Time.time >= lastTimeUsed + (1f / sender.getASPD()) ? true : false; 
            case SkillCoolDownType.CooldownSpell: break;
            case SkillCoolDownType.Spell: return Time.time >= lastTimeUsed + (1f / sender.getCastPerSecond()) ? true: false;
        }

        return false;
    }

    public void skillHasBeenUsed()
    {
        lastTimeUsed = Time.time;
    }
}
