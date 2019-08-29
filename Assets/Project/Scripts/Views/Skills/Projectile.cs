using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void ProjectileCollision(BeingBehavior sender, GameObject target, float value);
    public ProjectileCollision projectileCollisionDelegate;

    BeingBehavior _senderObject;
    List<EffectAndValue> _effectValues;

    public void setProjectile(BeingBehavior sender, List<EffectAndValue> effectAndValues)
    {
        this._senderObject = sender;
        this._effectValues = effectAndValues;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BeingBehavior>() != null)
        {
            for (int i = 0; i < _effectValues.Count; i++)
                _effectValues[i].effect.use(_senderObject, other.gameObject, _effectValues[i].value);
        }
        else if (other.CompareTag(Tags.Environment.ToString()))
        {
            Destroy(gameObject);
        }
    }
}
