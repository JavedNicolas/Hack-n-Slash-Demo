using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.abilityAttributsMenuName + "Lightning Ball", fileName = "LightningBallAttributs")]
public class LightningBallAttributs : AbilityAttributs, IProjectileAttributs
{
    [Header("Projectile")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] ProjectileFormType _formType;
    [SerializeField] float _offsetBetweenProjectile ;
    [SerializeField] float _projectileLife;
    [SerializeField] float _baseProjectileSpeed;

    public GameObject projectilePrefab => _projectilePrefab;
    public ProjectileFormType formType => _formType;
    public float offsetBetweenProjectile => _offsetBetweenProjectile;
    public float projectileLife => _projectileLife;
    public float baseProjectileSpeed => _baseProjectileSpeed;
}
