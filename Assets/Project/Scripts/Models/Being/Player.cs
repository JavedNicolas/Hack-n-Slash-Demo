using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
    #region attributs
    [Header("Inventory")]
    private Inventory _inventory = new Inventory();
    public Inventory inventory { get { return _inventory; } }

    [Header("Mana")]
    [SerializeField] private float _currentMana;
    public float currentMana { get => _currentMana; }

    public new PlayerStats stats { get => (PlayerStats)base.stats; }
    #endregion

    #region levelUp delegate
    public delegate void  HasLeveledUP();
    public HasLeveledUP hasLeveledUP;
    #endregion

    public Player(string name, float baseLife, float baseMana, float baseASPD, float baseCastSpeed, float baseAttackRange, List<int> abilityIDs, GameObject prefab, int currentLevel) :
        base(name, baseLife, baseASPD, baseCastSpeed, baseAttackRange, BeingConstant.baseMoveSpeed, abilityIDs, prefab)
    {
        base.stats = new PlayerStats(baseLife, baseMana, baseASPD, baseCastSpeed, baseAttackRange, BeingConstant.baseMoveSpeed);
        setLevelUpDelegate();
        this._currentMana = stats.maxMana;
        setBaseStat();
    }

    public Player(Player player) : base(player.name,
        player.stats.maxLife, player.stats.attackSpeed, player.stats.castSpeed, player.stats.attackRange, BeingConstant.baseMoveSpeed, player.abilityIDs, player.prefab)
    {
        base.stats = new PlayerStats(player.stats);
        setLevelUpDelegate();
        this._inventory = player.inventory;
        this._currentMana = player.currentMana;
        
        setBaseStat();
    }

    void setBaseStat()
    {
        // life and mana
        base.stats.addStat(new Stat(StatType.Life, StatBonusType.Pure, 10, BeingConstant.ClassBaseSourceName, StatInfluencedBy.Level));
        base.stats.addStat(new Stat(StatType.Mana, StatBonusType.Pure, 2, BeingConstant.ClassBaseSourceName, StatInfluencedBy.Level));

        // basic stat
        base.stats.addStat(new Stat(StatType.Intelligence, StatBonusType.Pure, 10, BeingConstant.ClassBaseSourceName, StatInfluencedBy.Level));
        base.stats.addStat(new Stat(StatType.Dexterity, StatBonusType.Pure, 1, BeingConstant.ClassBaseSourceName, StatInfluencedBy.Level));
        base.stats.addStat(new Stat(StatType.Strength, StatBonusType.Pure, 3, BeingConstant.ClassBaseSourceName, StatInfluencedBy.Level));
    }

    void setLevelUpDelegate()
    {
        
    }

    /// <summary>
    /// Spend an amount of mana
    /// </summary>
    /// <param name="mana">The mana to spend</param>
    public void spendMana(float mana)
    {
        _currentMana = Mathf.Clamp(_currentMana - mana, 0, stats.maxMana);
    }

    /// <summary>
    /// Add experience to the player and th
    /// </summary>
    /// <param name="value"></param>
    public void addAllExperience(float value)
    {
        addExperience(value);
        foreach (Ability ability in abilities)
        {
            ability.addExperience(value);
        }
    }

    public float getPlayerCurrentExpAsFloatPercentage()
    {
        int index = stats.currentLevel == LevelExperienceTable.levelExperienceNeeded.Length ? stats.currentLevel -1 : stats.currentLevel;
        return stats.currentLevelExp / LevelExperienceTable.levelExperienceNeeded[index];
    }

    /// <summary>
    /// Add experience to the player
    /// </summary>
    /// <param name="value">The experience to add</param>
    public void addExperience(float value)
    {
        // if this is not the last level then add the exp 
        //and level up if the current xp is above the level requirement
        if (stats.currentLevel != LevelExperienceTable.levelExperienceNeeded.Length)
        {
            stats.setCurrentExperience(stats.currentLevelExp + value);
            if (stats.currentLevelExp >= LevelExperienceTable.levelExperienceNeeded[stats.currentLevel])
                levelUp();
        }

        // if the level is the maxium then leave the currentXP at max
        if (stats.currentLevel == LevelExperienceTable.levelExperienceNeeded.Length)
            stats.setCurrentExperience(LevelExperienceTable.levelExperienceNeeded[stats.currentLevel - 1]);
    }

    void levelUp()
    {
        stats.levelUp();
        stats.setCurrentExperience(stats.currentLevelExp - LevelExperienceTable.levelExperienceNeeded[stats.currentLevel - 1]);

        _currentMana = stats.maxMana;
        _currentLife = stats.maxLife;

        hasLeveledUP();
    }

}
