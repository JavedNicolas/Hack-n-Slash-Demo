using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public abstract class Ability : DatabaseElement
{
    // attributs
    // handle the cooldown
    protected float lastTimeUsed = 0f;

    // Ability attributs
    [SerializeField] public AbilityAttributs abilityAttributs;

    // Stats
    [SerializeField] public Stats stats = new Stats();


    #region levelUp delegate
    public delegate void HasLeveledUP();
    public HasLeveledUP hasLeveledUP;
    #endregion

    public Ability()
    {
        setBaseStats();
        stats = new Stats();
    }

    public Ability(Ability ability)
    {
        abilityAttributs = ability.abilityAttributs;
        stats = new Stats(ability.stats);
        setBaseStats();
    }

    public void copyAttributs(Ability ability)
    {
        abilityAttributs = ability.abilityAttributs;
        stats = new Stats(ability.stats);
        setBaseStats();
    }

    #region exp and level
    float[] getAbilityExpTable()
    {
        float[] expTable;
        int indexStep = Mathf.CeilToInt((float)(LevelExperienceTable.levelExperienceNeeded.Length / 2) / (float)abilityAttributs.maxLevel);

        // if the level experience table does not have enought length then the expTable will shorten
        if(LevelExperienceTable.levelExperienceNeeded.Length < (abilityAttributs.maxLevel - 1)* indexStep)
            expTable = new float[LevelExperienceTable.levelExperienceNeeded.Length / indexStep];
        else
            expTable = new float[abilityAttributs.maxLevel];

        expTable[0] = 0;
        for (int i = 1; i < expTable.Length; i++)
        {
            int indexToUse = (abilityAttributs.levelNeeded + indexStep * i) -1;
            expTable[i] = LevelExperienceTable.levelExperienceNeeded[indexToUse];
        }

        return expTable;
    }

    /// <summary>
    /// Add experience to the player
    /// </summary>
    /// <param name="value">The experience to add</param>
    public void addExperience(float value)
    {
        stats.addExperience(value, getAbilityExpTable());
    }

    void levelUp(float[] expTable)
    {
        stats.levelUp();
        stats.setCurrentExperience(stats.currentLevelExp - expTable[stats.currentLevel - 1]);
        //hasLeveledUP();
    }
    #endregion

    /// <summary>
    /// Set the ability base stats
    /// </summary>
    public abstract void setBaseStats();

    /// <summary>
    /// Start the sender animation for this ability
    /// </summary>
    public abstract void animation();
    /// <summary>
    /// Perform Ability (used for targeted attack)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="target">The target gameObject</param>
    abstract public void performAbility(BeingBehavior sender, BeingBehavior target);

    /// <summary>
    /// Perform Ability (used for projectile)
    /// </summary>
    /// <param name="sender">The sender being model</param>
    /// <param name="targetedPosition">The ability targeted position</param>
    /// <param name="senderGameObject">The sender gameObject</param>
    abstract public void performAbility(BeingBehavior sender, Vector3 targetedPosition);

    /// <summary>
    /// Use every effect for the effectType
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="forEffectType"></param>
    public void useEffects(BeingBehavior sender, GameObject target, EffectUseBy forEffectType)
    {
        BeingBehavior targetBehavior = target.GetComponent<BeingBehavior>();

        if(targetBehavior != null && isCorrectTarget(sender, targetBehavior))
            foreach(AbilityEffectAndValue effectAndValue in abilityAttributs.effectAndValues)
            {
                if(effectAndValue.effectType == forEffectType)
                    effectAndValue.useEffect(sender, target, this);
            }
    }

    /// <summary>
    /// check if the target is valid
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="target">The target</param>
    /// <returns>return true if the target is correct</returns>
    public bool isCorrectTarget(BeingBehavior sender, BeingBehavior target)
    {
        BeingBehavior targetScript = target.GetComponent<BeingBehavior>();

        switch (abilityAttributs.targetType)
        {
            case BeingTargetType.Self:
                if (targetScript.being == sender.being)
                    return true;
                break;
            case BeingTargetType.Enemy:
                if (targetScript.teamID != sender.teamID)
                    return true;
                break;
            case BeingTargetType.Allies:
                if (targetScript.teamID == sender.teamID)
                    return true;
                break;
            case BeingTargetType.SelfAndAllies:
                if (targetScript.teamID == sender.teamID || targetScript.being == sender.being)
                    return true;
                break;
            case BeingTargetType.AllBeing:
                if (targetScript.teamID == sender.teamID || targetScript.teamID != sender.teamID || targetScript.being == sender.being)
                    return true;
                break;
        }

        return false;
    }

    /// <summary>
    /// Get all the effect a given effect Type
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public List<AbilityEffectAndValue> getEffectFor(EffectUseBy effectType)
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
        switch (abilityAttributs.coolDownType)
        {
            case AbilityCoolDownType.ASPD: return Time.time >= lastTimeUsed + (1f / sender.stats.attackSpeed) ? true : false;
            case AbilityCoolDownType.Cooldown: ICoolDownAttributs coolDownAttributs = (ICoolDownAttributs)abilityAttributs; return Time.time >= lastTimeUsed + coolDownAttributs.coolDown ? true : false;
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
        return abilityAttributs.icon;
    }

    public void setAttributs(AbilityAttributs attributs)
    {
        abilityAttributs = attributs;
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
        return GetType().Name;
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
