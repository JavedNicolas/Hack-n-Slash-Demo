using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillConstant
{
    public static string skillPrefabFolder = "Prefabs/Skills/";
    public static string projectilePrefabFolder = skillPrefabFolder + "Projectile/";
}

public enum SkillType 
{
    Regular, Projectile, SelfArea, MousePosition, InFront, line, Self
}

public enum SkillCoolDownType
{
    Attack, Spell, CooldownSpell
}

public enum ProjectileFormType
{
    Cone, Line
}



