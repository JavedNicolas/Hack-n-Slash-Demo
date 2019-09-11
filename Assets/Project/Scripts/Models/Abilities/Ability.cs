using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class Ability : DatabaseElement
{ 
    // attributs
    // handle the cooldown
    protected float lastTimeUsed = 0f;

    // Ability attributs
    [SerializeField] protected AbilityAttributs _abilityAttributs;
    public AbilityAttributs abilityAttributs { get => _abilityAttributs; }

    // Stats
    [SerializeField] protected Stats _stats = new Stats();
    public Stats stats { get => _stats; }

    #region levelUp delegate
    public delegate void HasLeveledUP();
    public HasLeveledUP hasLeveledUP;
    #endregion

    public Ability()
    {

    }

    public Ability(Ability ability)
    {
        _abilityAttributs = ability.abilityAttributs;
        _stats = ability.stats;
    }

    #region exp and level
    float[] getAbilityExpTable()
    {
        float[] expTable;
        int indexStep = abilityAttributs.levelNeeded + abilityAttributs.maxLevel / ((LevelExperienceTable.levelExperienceNeeded.Length) / 2);

        // if the level experience table does not have enought length then the expTable will shorten
        if(LevelExperienceTable.levelExperienceNeeded.Length < (abilityAttributs.maxLevel - 1)* indexStep)
            expTable = new float[LevelExperienceTable.levelExperienceNeeded.Length / indexStep + 1];
        else
            expTable = new float[abilityAttributs.maxLevel];

        expTable[0] = 0;
        for (int i = 1; i < expTable.Length; i++)
        {
            expTable[i] = LevelExperienceTable.levelExperienceNeeded[indexStep + i];
        }

        return expTable;
    }

    /// <summary>
    /// Add experience to the player
    /// </summary>
    /// <param name="value">The experience to add</param>
    public void addExperience(float value)
    {
        float[] expTable = getAbilityExpTable();

        // if this is not the last level then add the exp 
        //and level up if the current xp is above the level requirement
        if (stats.currentLevel != expTable.Length)
        {
            stats.setCurrentExperience(stats.currentLevelExp + value);
            if (stats.currentLevelExp >= expTable[stats.currentLevel])
                if(stats.currentLevel < abilityAttributs.maxLevel)
                    levelUp(expTable);
        }

        // if the level is the maxium then leave the currentXP at max
        if (stats.currentLevel == expTable.Length)
        {
            stats.setCurrentExperience(expTable[stats.currentLevel - 1]);
        }
    }

    void levelUp(float[] expTable)
    {
        stats.levelUp();
        stats.setCurrentExperience(stats.currentLevelExp - expTable[stats.currentLevel - 1]);
        hasLeveledUP();
    }
    #endregion

    /// <summary>
    /// Set the ability base stats
    /// </summary>
    public abstract void setBaseStats();

    /// <summary>
    /// Start the sender animation for this ability
    /// </summary>
    abstract public void animation();

    /// <summary>
    /// Perform Ability (used for targeted attack)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targetGameObject">The target gameObject</param>
    abstract public void performAbility(BeingBehavior sender, BeingBehavior targetGameObject);

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    abstract public void performAbility(BeingBehavior sender, Vector3 targedPosition);

    /// <summary>
    /// Use every effect for the effectType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="forEffectType"></param>
    public void useEffects(BeingBehavior sender, GameObject target, EffectType forEffectType)
    {
        foreach(AbilityEffectAndValue effectAndValue in abilityAttributs.effectAndValues)
        {
            if(effectAndValue.effectType == forEffectType)
                effectAndValue.useEffect(sender, target, this);
        }
    }

    /// <summary>
    /// Get all the effect a given effect Type
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public List<AbilityEffectAndValue> getEffectFor(EffectType effectType)
    {
        return abilityAttributs.effectAndValues.FindAll(x => x.effectType == effectType).ToList();
    }

    /// <summary>
    /// Check if the ability if available (cooldown type)
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public bool isAbilityAvailable(Being sender)
    {
        switch (_abilityAttributs.coolDownType)
        {
            case AbilityCoolDownType.ASPD: return Time.time >= lastTimeUsed + (1f / sender.stats.attackSpeed) ? true : false;
            case AbilityCoolDownType.Cooldown: break;
            case AbilityCoolDownType.CastSpeed: return Time.time >= lastTimeUsed + (1f / sender.stats.castSpeed) ? true : false;
        }

        return false;
    }

    /// <summary>
    /// retrived the icon of the ability
    /// </summary>
    /// <returns>A sprite of the icon</returns>
    public Sprite getIcon()
    {
        return _abilityAttributs.icon;
    }

    public void setAttributs(AbilityAttributs attributs)
    {
        _abilityAttributs = attributs;
    }

    /// <summary>
    /// Sert the ability as used and thus start the cooldown (or apsd/ castspeed cooldown)
    /// </summary>
    public void abilityHasBeenUsed()
    {
        lastTimeUsed = Time.time;
    }

    public override string getName()
    {
        return GetType().ToString();
    }

    /// <summary>
    /// Return the description based on the effect
    /// </summary>
    /// <param name="owner">The owner of the ability (to get the buffedValues)</param>
    /// <returns></returns>
    public override string getDescription(Being owner)
    {
        return abilityAttributs.description;
    }
}
