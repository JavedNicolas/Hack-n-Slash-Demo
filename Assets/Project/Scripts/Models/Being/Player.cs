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

    [SerializeField] private float _baseMana;
    public float baseMana { get => _baseMana; }

    [SerializeField] private float _currentMana;
    public float currentMana { get => _currentMana; }

    #endregion

    #region delegate
    public delegate void HasLevelUp();
    public HasLevelUp hasLevelUp;
    #endregion

    public Player(string name, float baseLife, float baseMana, float shield, float aSPD, float attackRange, int strength, List<Ability> skills, float movementSpeedPercentage, GameObject prefab, float projectileSpeed) : base(name, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage, prefab, projectileSpeed)
    {
        this._baseMana = baseMana;
        this._currentMana = getCurrentMaxMana();
        this._currentLevel = 1;
    }

    public Player(Player player)
    {
        this._currentLevelExp = player.currentLevelExp;
        this._currentLevel = player.currentLevel;
        this._inventory = player.inventory;
        this._baseMana = player.baseMana;
        this._currentMana = player.currentMana;
        this._currentLevel = player.currentLevel;
    }

    /// <summary>
    /// Spend an amount of mana
    /// </summary>
    /// <param name="mana">The mana to spend</param>
    public void spendMana(float mana)
    {
        _currentMana = Mathf.Clamp(_currentMana - mana, 0, getCurrentMaxMana());
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
        hasLevelUp();
    }

    // getter
    /// <summary>
    /// Get the current amout of mana max
    /// </summary>
    /// <returns></returns>
    public float getCurrentMaxMana()
    {
        return _baseMana;
    }
}
