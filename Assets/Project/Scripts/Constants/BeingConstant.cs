using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeingConstant
{
    public static float distanceAggroFactor = 5f;
    public static float damageAggroFactor = 10f;
    public static float outOfRangeTargetTimer = 10f;
    public static float baseMoveSpeed = 10f;
    public static string ClassBaseSourceName = "Base Class Stat";
}

[System.Serializable]
public enum AbilityUsageFrequence
{
    VeryRare = 5,
    PrettyRare = 10,
    Rare = 25,
    Normal = 60,
    Frequent = 101
}