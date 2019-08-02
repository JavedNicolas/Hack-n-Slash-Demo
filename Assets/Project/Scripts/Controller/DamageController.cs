using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController 
{

    #region singleton
    public static DamageController instance = new DamageController();

    private DamageController() { }
    #endregion

    /// <summary>
    /// Apply damage to a being
    /// </summary>
    /// <param name="target">Target which will receive the damage</param>
    /// <param name="damage">The damage to apply</param>
    public void applyDamage(Being target, float damage)
    {
        target.currentLife = Mathf.Clamp(target.currentLife - damage, 0, target.getCurrentMaxLife());
    }

}
