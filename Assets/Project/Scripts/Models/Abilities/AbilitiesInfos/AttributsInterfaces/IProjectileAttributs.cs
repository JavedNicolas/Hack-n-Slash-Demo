using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IProjectileAttributs
{
    GameObject projectilePrefab { get; }
    ProjectileFormType formType { get;  }
    float offsetBetweenProjectile { get; }
    float projectileLife { get; }
    float baseProjectileSpeed { get; }
}
