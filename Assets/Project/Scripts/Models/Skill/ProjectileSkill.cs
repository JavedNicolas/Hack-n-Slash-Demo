using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileSkill : Skill
{
    public GameObject model;
    public float projectileBaseSpeed = 25;
    public abstract int numberOfProjectile { get; }
    public abstract ProjectileFormType projectileFormType { get; }
    public abstract float offsetBetweenProjectile { get; }

    public int getNumberOfProjectile(Being sender)
    {
        return numberOfProjectile;
    }
}
