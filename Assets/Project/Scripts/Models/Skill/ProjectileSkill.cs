using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkill : Skill
{
    public GameObject model;
    public float projectileBaseSpeed = 25;
    public int numberOfProjectile;
    public ProjectileFormType projectileFormType;
    public float offsetBetweenProjectile;

    public int getNumberOfProjectile(Being sender)
    {
        return numberOfProjectile;
    }
}
