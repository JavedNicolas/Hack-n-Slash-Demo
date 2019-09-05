using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbilityAttributs: AbilityAttributs
{
    [Header("Projectile")]
    [SerializeField] private GameObject _ballGameObject;
    [SerializeField] private int _numberOfBaseProjectile;
    [SerializeField] private float _projectileBaseSpeed;
    [SerializeField] private ProjectileFormType _formType;
    [SerializeField] private float _offsetBetweenProjectile;
    [SerializeField] private float _projectileLife;

    public GameObject ballGameObject { get => _ballGameObject;}
    public int numberOfBaseProjectile { get => _numberOfBaseProjectile;}
    public float projectileBaseSpeed { get => _projectileBaseSpeed; }
    public ProjectileFormType formType { get => _formType; }
    public float offsetBetweenProjectile { get => _offsetBetweenProjectile;  }
    public float projectileLife { get => _projectileLife;  }

    public int getNumberOfProjectile()
    {
        return numberOfBaseProjectile;
    }

    public float getProjectileLifve()
    {
        return projectileLife;
    }

    public float getProjectileSpeed()
    {
        return projectileBaseSpeed;
    }
}
