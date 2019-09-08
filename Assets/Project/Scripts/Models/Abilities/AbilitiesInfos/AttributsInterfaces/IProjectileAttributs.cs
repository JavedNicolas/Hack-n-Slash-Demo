using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IProjectileAttributs
{
    [SerializeField] GameObject ballGameObject { get; }
    [SerializeField] int numberOfBaseProjectile { get; }
    [SerializeField] float projectileBaseSpeed { get; }
    [SerializeField] ProjectileFormType formType { get;  }
    [SerializeField] float offsetBetweenProjectile { get; }
    [SerializeField] float projectileLife { get; }
    [SerializeField] List<EffectAndValue> effectOfTheProjectile { get; }

    void sendProjectile(BeingBehavior sender, Vector3 targetedPosition, string abilityName);

}
