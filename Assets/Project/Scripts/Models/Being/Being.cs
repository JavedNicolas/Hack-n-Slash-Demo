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

    [Header("Base Stat")]
    [SerializeField] float _maxLife;
    public float maxLife { get { return Mathf.FloorToInt(getBuffedValue(_maxLife, StatType.Life)); } }

    [SerializeField] float _attackRange;
    public float attackRange { get => getBuffedValue(_attackRange, StatType.AttackRange); }

    [SerializeField] float _movementSpeed;
    public float movementSpeed { get => getBuffedValue(_movementSpeed, StatType.MovementSpeed); }

    [SerializeField] float _attackSpeed;
    public float attackSpeed { get => getBuffedValue(_attackSpeed, StatType.AttackSpeed); }

    [SerializeField] float _castSpeed;
    public float castSpeed { get => getBuffedValue(_castSpeed, StatType.CastSpeed); }

    [Header("Stats")]

    [SerializeField] protected int _intelligence;
    public int intelligence { get => getBuffedValue(_intelligence, StatType.Intelligence); }

    [SerializeField] protected int _strength;
    public int strength { get => getBuffedValue(_strength, StatType.Strength); }

    [SerializeField] protected int _dexterity;
    public int dexterity { get => getBuffedValue(_dexterity, StatType.Dexterity); }

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
        this._maxLife = baseLife;
        this._attackSpeed = baseASPD;
        this._castSpeed = baseCastSpeed;
        this._attackRange = baseAttackRange;
        this._movementSpeed = baseMovementSpeed;
        this._ability = abilities;
        this._prefab = prefab;
        this._basicAttack = new BasicAttack();
        this._currentLife = maxLife;
    }

    public Being() { }

    # endregion

    /// <summary>
    /// Apply damage to the being
    /// </summary>
    /// <param name="damage">the damage to apply</param>
    public void takeDamage(float damage)
    {
        _currentLife = Mathf.Clamp(_currentLife - damage, 0, maxLife);
    }

    public void heal(float life)
    {
        _currentLife = Mathf.Clamp(_currentLife + life, 0, maxLife);
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
        float pureBonus = 0;
        float additionalBonus = 0;
        float multipliedBonus = 0;

        for(int i =0; i < stats.Count; i++)
        {
            Stat currentStat = stats[i];
            if(currentStat.statType == statType)
            {
                float influenceFactor = getInflencedFactor(currentStat);
                influenceFactor = Mathf.FloorToInt(influenceFactor / currentStat.influencedEvery);
                switch (currentStat.bonusType)
                {
                    case StatBonusType.Pure: pureBonus += currentStat.value * influenceFactor; break;
                    case StatBonusType.additional: additionalBonus += currentStat.value * influenceFactor; break;
                    case StatBonusType.Multiplied: multipliedBonus += currentStat.value * influenceFactor; break;
                }
            }
           
        }

        float buffedValue = ((value + pureBonus) + ((value + pureBonus) * (additionalBonus / 100))) * (1 + (multipliedBonus /100));

        return buffedValue;
    }

    protected virtual float getInflencedFactor(Stat stat)
    {
        switch (stat.isInfluencedBy)
        {
            case StatInfluencedBy.Nothing: return 1;
            case StatInfluencedBy.Intelligence: return intelligence ;
            case StatInfluencedBy.Strength: return strength;
            case StatInfluencedBy.Dexterity: return dexterity;
            case StatInfluencedBy.MaxLife: return maxLife;
            default: return 0;
        }
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
