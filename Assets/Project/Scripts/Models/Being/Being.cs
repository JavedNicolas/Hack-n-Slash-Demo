using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Being : Interactable
{
    #region being attributs

    [Header("Current")]
    protected float _currentLife;
    public float currentLife { get { return _currentLife; } }

    [Header("Base Start")]
    [SerializeField] float _baseLife;
    public float baseLife { get { return _baseLife; } }

    [SerializeField] float _baseAttackRange;
    public float baseAttackRange { get => _baseAttackRange; }

    [SerializeField] float _baseMovementSpeed;
    public float baseMovementSpeed { get => _baseMovementSpeed; }

    [SerializeField] float _baseAttackSpeed;
    public float baseAttackSpeed { get => _baseAttackSpeed; }

    [SerializeField] float _baseCastSpeed;
    public float baseCastSpeed { get => _baseAttackSpeed; }

    [Header("Stats")]
    [SerializeField] protected List<Stat> _stats = new List<Stat>();
    public List<Stat> stats { get => _stats; }

    [Header("Skills")]
    [SerializeField] private List<Ability> _ability = new List<Ability>();
    public List<Ability> abilities { get { return _ability; } }

    private BasicAttack _basicAttack;
    public BasicAttack basicAttack{ get { return _basicAttack;}}

    [Header("Game Object")]
    [SerializeField] private GameObject _prefab;
    public GameObject prefab {  get { return _prefab; } }

    #endregion

    #region init
    public Being(string name, float baseLife, float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed, List<Ability> abilities, GameObject prefab)
    {
        this.name = name;
        this._baseLife = baseLife;
        this._baseAttackSpeed = baseASPD;
        this._baseCastSpeed = baseCastSpeed;
        this._baseAttackRange = baseAttackRange;
        this._baseMovementSpeed = baseMovementSpeed;
        this._ability = abilities;
        this._prefab = prefab;
        this._basicAttack = new BasicAttack();
        this._currentLife = getCurrentMaxLife();
    }

    public Being() { }

    # endregion

    /// <summary>
    /// Apply damage to the being
    /// </summary>
    /// <param name="damage">the damage to apply</param>
    public void takeDamage(float damage)
    {
        _currentLife = Mathf.Clamp(_currentLife - damage, 0, getCurrentMaxLife());
    }

    public void heal(float life)
    {
        _currentLife = Mathf.Clamp(_currentLife + life, 0, getCurrentMaxLife());
    }

    /// <summary>
    /// Get the attack per second value
    /// </summary>
    /// <returns>The Attack per second</returns>
    public float getAttackRange()
    {
        return getBuffedValue(_baseAttackRange, StatType.AttackRange);
    }

    /// <summary>
    /// Get the attack per second value
    /// </summary>
    /// <returns>The Attack per second</returns>
    public float getASPD()
    {
        return getBuffedValue(_baseAttackSpeed, StatType.AttackSpeed);
    }

    /// <summary>
    /// Get the cast per second value
    /// </summary>
    /// <returns>The cast per second</returns>
    public float getCastPerSecond()
    {
        return getBuffedValue(_baseAttackSpeed, StatType.CastSpeed);
    }

    /// <summary>
    /// get the movement speed
    /// </summary>
    /// <returns>the movement speed</returns>
    public virtual float getMovementSpeed()
    {
        return getBuffedValue(_baseMovementSpeed, StatType.MovementSpeed);
    }

    /// <summary>
    /// Get the curent max life (base life + all the bonuses)
    /// </summary>
    /// <returns></returns>
    public float getCurrentMaxLife()
    {
        return getBuffedValue((int)_baseLife, StatType.Life);
    }

    /// <summary>
    /// Return the value buffed by the player stats as an int
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns>An ceiled int</returns>
    public int getBuffedValue(int value, StatType statType)
    {
        return Mathf.FloorToInt(getBuffedValue((float)value, statType));
    }

    /// <summary>
    /// Return the value buffed by the player stats
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns></returns>
    public float getBuffedValue(float value, StatType statType)
    {
        float buffedValue = 0;
        float pureBonus = 0;
        float additionalBonus = 0;
        float multipliedBonus = 0;

        for(int i =0; i < stats.Count; i++)
        {
            Stat currentStat = stats[i];
            if(currentStat.statType == statType)
            switch (currentStat.bonusType)
            {
                case StatBonusType.Pure: pureBonus += currentStat.value; break;
                case StatBonusType.additional: additionalBonus += currentStat.value; break;
                case StatBonusType.Multiplied: multipliedBonus += currentStat.value; break;
            }
        }

        buffedValue = ((value + pureBonus) + ((value + pureBonus) * (additionalBonus / 100))) * (1 + (multipliedBonus /100));

        return buffedValue;
    }

    /// <summary>
    /// try to add a stat to the player
    /// </summary>
    /// <param name="stat">The stat to add</param>
    /// <returns>Return true if the stat has be added, false if not. \n
    /// Ex : It can fail if the player cannot get this stat, or if it already has it from the same source)</returns>
    public bool addStat(Stat stat)
    {
        if (_stats.Exists(x => x.sourceName == stat.sourceName && x.bonusType == stat.bonusType && x.statType == stat.statType))
            return false;

        _stats.Add(stat);

        return true;
    }


    /// <summary>
    /// try to remove a stat to the player
    /// </summary>
    /// <param name="stat">The stat to remove</param>
    /// <returns>Return true if the stat has be removed, false if not. \n
    /// Ex : The player does not have this stat from this source</returns>
    public bool removeStat(string sourceName)
    {
        List<Stat> statsFound = _stats.FindAll(x => x.sourceName == sourceName);
        if(statsFound.Count == 0)
            return false;

        for(int i=0; i < statsFound.Count; i++)
        {
            if(_stats.Contains(statsFound[i]))
            {
                _stats.Remove(statsFound[i]);
            }
        }

        return true;
    }

    /// <summary>
    /// return true if the enemy is dead
    /// </summary>
    /// <returns></returns>
    public bool isDead()
    {
        return _currentLife <= 0;
    }

    /// <summary>
    /// Interaction with the being
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="objectToInteractWith"></param>
    /// <returns></returns>
    public override bool interact(BeingBehavior sender, GameObject objectToInteractWith)
    {
        if (objectToInteractWith.GetComponent<BeingBehavior>() != null && objectToInteractWith.GetComponent<BeingBehavior>().teamID != sender.teamID)
        {
            sender.useAbility(basicAttack, objectToInteractWith.transform.position, objectToInteractWith.GetComponent<BeingBehavior>());
            return true;
        }

        return false ;
    }
}
