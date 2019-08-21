using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityConstant
{
    public static string skillPrefabFolder = "Prefabs/Skills/";
    public static string projectilePrefabFolder = skillPrefabFolder + "Projectile/";
    public static string attributsFolder = "ScriptableObjects/AbilityAttributs/";
}

public enum AbilityStartPosition
{
    AroundSelf, MousePosition, InFront, OnTarget
}

public enum AbilityCoolDownType
{
    ASPD, CastSpeed, Cooldown
}

public enum ProjectileFormType
{
    Cone, Line
}

public enum AbilityType
{
    Attack, Spell, Support
}



