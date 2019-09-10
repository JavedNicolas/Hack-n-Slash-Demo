using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void ProjectileCollision(BeingBehavior sender, GameObject target, float value);
    public ProjectileCollision projectileCollisionDelegate;

    protected BeingBehavior _senderBehavior;
    protected Ability _origin;
    protected List<AbilityEffectAndValue> _effectAndValues = new List<AbilityEffectAndValue>();

    public void setProjectile(BeingBehavior sender, Ability origin, List<AbilityEffectAndValue> effectAndValues)
    {
        this._senderBehavior = sender;
        this._origin = origin;
        this._effectAndValues = effectAndValues;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BeingBehavior>() != null)
        {
            BeingBehavior targetBehavior = other.GetComponent<BeingBehavior>();
            if (targetBehavior.teamID != _senderBehavior.teamID)
                projectileEffect(targetBehavior);
        }
        else if (other.CompareTag(Tags.Environment.ToString()))
        {
            Destroy(gameObject);
        }
    }

    public virtual void projectileEffect(BeingBehavior targetBehavior)
    {
        foreach(AbilityEffectAndValue effectAndValue in _effectAndValues)
        {
            effectAndValue.useEffect(_senderBehavior, targetBehavior.gameObject, _origin);
        }
    }
}
