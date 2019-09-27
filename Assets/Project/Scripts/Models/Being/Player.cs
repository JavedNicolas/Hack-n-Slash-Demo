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

    public Player(string name, float baseLife, float baseMana, float baseASPD, float baseCastSpeed, float baseAttackRange, List<int> abilityIDs, GameObject prefab, int currentLevel = 1) :
        base(name, baseLife, baseASPD, baseCastSpeed, baseAttackRange, BeingConstant.baseMoveSpeed, abilityIDs, prefab)
    {
        base.stats = new PlayerStats(baseLife, baseMana, baseASPD, baseCastSpeed, baseAttackRange, BeingConstant.baseMoveSpeed, currentLevel);
        this._currentMana = stats.maxMana;
        setBaseStat();
        setLevelUpDelegate();
        setCurrentManaAndLife();
    }

    public Player(Player player) : base(player.name,
        player.stats.maxLife, player.stats.attackSpeed, player.stats.castSpeed, player.stats.attackRange, BeingConstant.baseMoveSpeed, player.abilityIDs, player.model)
    {
        base.stats = new PlayerStats(player.stats);
        this._inventory = player.inventory;
        setBaseStat();
        setLevelUpDelegate();
        setCurrentManaAndLife();
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
        stats.hasLeveledUP += setCurrentManaAndLife;
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
        stats.addExperience(value, LevelExperienceTable.levelExperienceNeeded);
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

    void setCurrentManaAndLife()
    {
        _currentMana = stats.maxMana;
        _currentLife = stats.maxLife;
    }

}
