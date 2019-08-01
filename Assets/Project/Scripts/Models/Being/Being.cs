using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Being : ScriptableObject
{
    [Header("Health")]
    public float life;
    public float shield;

    [Header("Damage")]
    public float attackSpeed;
    public float attackRange;

    [Header("Stats")]
    public float strength;

    [Header("Misc")]
    public float movementSpeedPercentage = 100;

}
