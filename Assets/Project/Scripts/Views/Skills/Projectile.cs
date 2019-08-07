using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void ProjectileCollision(Being target, Being sender);
    public ProjectileCollision projectileCollisionDelegate;

    public BeingBehavior senderObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BeingBehavior>() != null)
        {
            Being beingTouched = other.GetComponent<BeingBehavior>().being;
            if(!other.CompareTag(senderObject.tag))
            {
                projectileCollisionDelegate(beingTouched, senderObject.being);
            }
        }else
        {
            if(other.CompareTag(Tags.Environment.ToString()))
                Destroy(gameObject);
        }
    }
}
