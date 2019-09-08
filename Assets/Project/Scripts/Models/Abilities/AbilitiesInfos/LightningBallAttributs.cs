using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptableObjectConstant.abilityAttributsMenuName + "Lightning Ball", fileName = "Lightning Ball")]
public class LightningBallAttributs : AbilityAttributs, IProjectileAttributs
{
    [SerializeField] private GameObject _ballGameObject;
    [SerializeField] private int _numberOfBaseProjectile;
    [SerializeField] private float _projectileBaseSpeed;
    [SerializeField] private ProjectileFormType _formType;
    [SerializeField] private float _offsetBetweenProjectile;
    [SerializeField] private float _projectileLife;
    [SerializeField] private List<EffectAndValue> _effectOfTheProjectile;

    public GameObject ballGameObject { get => _ballGameObject;  }
    public int numberOfBaseProjectile { get => _numberOfBaseProjectile; }
    public float projectileBaseSpeed { get => _projectileBaseSpeed; }
    public ProjectileFormType formType { get => _formType; set => _formType = value; }
    public float offsetBetweenProjectile { get => _offsetBetweenProjectile; }
    public float projectileLife { get => _projectileLife;}
    public List<EffectAndValue> effectOfTheProjectile { get => _effectOfTheProjectile; }

    public void sendProjectile(BeingBehavior sender, Vector3 targetedPosition, string abilityName)
    {
        PlayerStat senderStats = ((Player)sender.being).stats;

        sender.abilityManager.launchProjectile(ballGameObject, senderStats.getBuffedValue(numberOfBaseProjectile, StatType.NumberOfProjectile, abilityName),
            offsetBetweenProjectile, senderStats.getBuffedValue(projectileBaseSpeed, StatType.ProjectileSpeed, abilityName), projectileLife, targetedPosition,
            formType, _effectOfTheProjectile);
    }
}
