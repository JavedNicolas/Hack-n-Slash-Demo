using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Area : MonoBehaviour
{
    // particule which that make the area
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] protected ParticleSystem areaSizeController;
    [SerializeField] CapsuleCollider areaCollider;
    [SerializeField] RadiusDisplayer radiusDisplayer;

    [SerializeField] float lifeduration;
    [SerializeField] float delayBeforeEffect;

    private Ability _origin;
    private List<AbilityEffectAndValue> _abilityEffectAndValues;
    private BeingBehavior _senderBehavior;

    private List<BeingBehavior> beingInArea = new List<BeingBehavior>();

    private void Update()
    {
        clearDeadBeingInArea();
    }

    public void setArea(Ability origin, List<AbilityEffectAndValue> abilityEffectAndValues, BeingBehavior senderBehavior)
    {
        this._origin = origin;
        this._abilityEffectAndValues = abilityEffectAndValues;
        this._senderBehavior = senderBehavior;
        increaseArea();

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Play();
        }

        // Invoke the correct recurrence based on the area Damage type
        IAreaAttributs areaAttributs = (IAreaAttributs)_origin.abilityAttributs;
        if (areaAttributs.areaDamageType == AreaType.Burst)
            burstDamage();
        else if(areaAttributs.areaDamageType == AreaType.DamageOverTime)
            overTimeDamage(areaAttributs.degenDelay);

        Destroy(gameObject, lifeduration);
    }

    private void OnTriggerEnter(Collider other)
    {
        IAreaAttributs areaAttributs = (IAreaAttributs)_origin.abilityAttributs;
        if (other.GetComponent<BeingBehavior>() != null)
        {
            BeingBehavior targetBehavior = other.GetComponent<BeingBehavior>();
            if(!beingInArea.Contains(targetBehavior) && targetBehavior.teamID != _senderBehavior.teamID)
                beingInArea.Add(targetBehavior);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<BeingBehavior>() != null)
        {
            BeingBehavior targetBehavior = other.GetComponent<BeingBehavior>();
            if (beingInArea.Contains(targetBehavior) && targetBehavior.teamID != _senderBehavior.teamID)
            {
                beingInArea.Remove(targetBehavior);
            }
        }
    }

    /// <summary>
    /// Apply a burst damage after the correct delay
    /// </summary>
    protected void burstDamage()
    {
        Invoke("applyAreaEffects", delayBeforeEffect);
    }

    /// <summary>
    /// Apply a Damage over time after the correct delay
    /// </summary>
    /// <param name="overTimeDamageDelay">The delay between every damage</param>
    protected void overTimeDamage(float overTimeDamageDelay)
    {
        float buffedDamageDelay = _senderBehavior.being.stats.getBuffedValue(overTimeDamageDelay, StatType.AreaOverTimeDelay, _origin.getName(), _origin.stats.statList);
        InvokeRepeating("applyAreaEffects", delayBeforeEffect, buffedDamageDelay);
    }

    /// <summary>
    /// remove dead enemy in area
    /// </summary>
    private void clearDeadBeingInArea()
    {
        for (int i = 0; i < beingInArea.Count; i++)
            if (beingInArea[i] == null || beingInArea[i].being.isDead())
                beingInArea.RemoveAt(i);
    }

    /// <summary>
    /// Apply the effect to every being still in the area
    /// </summary>
    protected virtual void applyAreaEffects()
    {
        for (int i = 0; i < beingInArea.Count; i++)
            if (beingInArea[i] != null && !beingInArea[i].being.isDead())
                foreach (AbilityEffectAndValue effectAndValue in _abilityEffectAndValues)
                    effectAndValue.useEffect(_senderBehavior, beingInArea[i].gameObject, _origin);
    }

    protected virtual void increaseArea()
    {
        if (areaCollider != null && areaSizeController != null)
        {
            areaCollider.radius = _senderBehavior.being.stats.getBuffedValue(areaCollider.radius, StatType.AreaSize, _origin.getName(), _origin.stats.statList);
            var main = areaSizeController.main;
            main.startSize = _senderBehavior.being.stats.getBuffedValue(main.startSize.constant, StatType.AreaSize, _origin.getName(), _origin.stats.statList);
            if (radiusDisplayer != null)
                radiusDisplayer.updateRadius(_senderBehavior.being.stats.getBuffedValue(radiusDisplayer.radius, StatType.AreaSize, _origin.getName(), _origin.stats.statList));
        }
            
    }
}
