using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
    #region attributs
    [Header("Levels")]
    private float _currentLevelExp;
    public float currentLevelExp { get => _currentLevelExp; }

    private int _currentLevel;
    public int currentLevel { get => _currentLevel; }

    [Header("Inventory")]
    private Inventory _inventory = new Inventory();
    public Inventory inventory { get { return _inventory; } }

    [Header("Mana")]

    [SerializeField] private float _maxMana;
    public float maxMana { get => Mathf.FloorToInt(getBuffedValue(_maxMana, StatType.Mana)); }

    [SerializeField] private float _currentMana;
    public float currentMana { get => _currentMana; }

    #endregion

    #region delegate
    public delegate void HasLevelUp();
    public HasLevelUp hasLevelUp;
    #endregion


    public Player(string name, float baseLife, float baseMana, float baseASPD, float baseCastSpeed, float baseAttackRange, List<Ability> abilities, GameObject prefab) :
        base(name, baseLife, baseASPD, baseCastSpeed, baseAttackRange, BeingConstant.baseMoveSpeed, abilities, prefab)
    {
        this._maxMana = baseMana;
        this._currentMana = getCurrentMaxMana();
        this._currentLevel = 1;
        setBaseStat();
    }

    public Player(Player player) : base(player.name, 
        player.maxLife, player.attackSpeed, player.castSpeed, player.attackRange, BeingConstant.baseMoveSpeed, player.abilities, player.prefab)
    {
        this._currentLevelExp = player.currentLevelExp;
        this._currentLevel = player.currentLevel;
        this._inventory = player.inventory;
        this._maxMana = player.maxMana;
        this._currentMana = player.currentMana;
        this._currentLevel = player.currentLevel;
        setBaseStat();
    }

    void setBaseStat()
    {
        // life and mana
        _stats.Add(new Stat(StatType.Life, StatBonusType.Pure, 10, "Base Class Stat", StatInfluencedBy.Level));
        _stats.Add(new Stat(StatType.Mana, StatBonusType.Pure, 2, "Base Class Stat", StatInfluencedBy.Level));

        // basic stat
        _stats.Add(new Stat(StatType.Intelligence, StatBonusType.Pure, 10, "Base Class Stat", StatInfluencedBy.Level));
        _stats.Add(new Stat(StatType.Dexterity, StatBonusType.Pure, 1, "Base Class Stat", StatInfluencedBy.Level));
        _stats.Add(new Stat(StatType.Strength, StatBonusType.Pure, 3, "Base Class Stat", StatInfluencedBy.Level));

        _stats.Add(new Stat(StatType.CastSpeed, StatBonusType.Pure, 1, "Base Class Stat", StatInfluencedBy.Dexterity));
    }

    /// <summary>
    /// Spend an amount of mana
    /// </summary>
    /// <param name="mana">The mana to spend</param>
    public void spendMana(float mana)
    {
        _currentMana = Mathf.Clamp(_currentMana - mana, 0, getCurrentMaxMana());
    }

    // getter
    /// <summary>
    /// Get the current amout of mana max
    /// </summary>
    /// <returns></returns>
    public float getCurrentMaxMana()
    {
        return getBuffedValue((int)_maxMana, StatType.Mana);
    }

    /// <summary>
    /// Add experience to the player
    /// </summary>
    /// <param name="value">The experience to add</param>
    public void addExperience(float value)
    {
        // if this is not the last level then add the exp 
        //and level up if the current xp is above the level requirement
        if (_currentLevel != LevelExperienceTable.levelExperienceNeeded.Length)
        { 
            _currentLevelExp += value;
            if (_currentLevelExp >= LevelExperienceTable.levelExperienceNeeded[currentLevel - 1])
            {
                levelUp();
            }
        }

        // if the level is the maxium then leave the currentXP at max
        if (_currentLevel == LevelExperienceTable.levelExperienceNeeded.Length)
        {
            _currentLevelExp = LevelExperienceTable.levelExperienceNeeded[currentLevel - 1];
        }
    }

    void levelUp()
    {
        _currentLevelExp = _currentLevelExp - LevelExperienceTable.levelExperienceNeeded[currentLevel - 1];
        _currentLevel++;

        _currentMana = maxMana;
        _currentLife = maxLife;

        hasLevelUp();
    }

    protected override float getInflencedFactor(Stat stat)
    {
        switch (stat.isInfluencedBy)
        {
            case StatInfluencedBy.Level: return _currentLevel;
            case StatInfluencedBy.JobLevel: return _currentLevel;
            case StatInfluencedBy.MaxMana: return getCurrentMaxMana();
            default: return base.getInflencedFactor(stat);
        }
    }
}
