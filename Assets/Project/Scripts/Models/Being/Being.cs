using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Being
{
    [Header("Name")]
    public string name;

    [Header("Health")]
    public float currentLife;
    public float baseLife;
    public float shield;

    [Header("Damage")]
    public float aspd;
    public float attackRange;

    [Header("Stats")]
    public float strength;

    [Header("Skills")]
    public List<Skill> skills = new List<Skill>();

    [Header("Game Object")]
    public GameObject prefab;

    [Header("Misc")]
    public float movementSpeedPercentage = 100;

    public Being(float currentLife, float baseLife, float shield, float aSPD, float attackRange, float strength, List<Skill> skills, float movementSpeedPercentage)
    {
        this.currentLife = currentLife;
        this.baseLife = baseLife;
        this.shield = shield;
        aspd = aSPD;
        this.attackRange = attackRange;
        this.strength = strength;
        this.skills = skills;
        this.movementSpeedPercentage = movementSpeedPercentage;

    }

    public Being() { }

    public float getCurrentMaxLife()
    {
        return baseLife;
    }



}
