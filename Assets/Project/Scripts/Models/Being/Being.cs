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

    [Header("Stats")]
    [SerializeField] protected BeingStats _stats = new BeingStats();
    public BeingStats stats { get => _stats; }

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
    public Being(string name, float baseLife,  float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed, List<Ability> abilities, GameObject prefab)
    {
        this.name = name;
        this._stats = new BeingStats(baseLife, baseASPD, baseCastSpeed, baseAttackRange, baseMovementSpeed, 1);
        this._ability = abilities;
        this._prefab = prefab;
        this._basicAttack = new BasicAttack();
        this._currentLife = _stats.maxLife;
    }

    public Being() { }

    # endregion

    /// <summary>
    /// Apply damage to the being
    /// </summary>
    /// <param name="damage">the damage to apply</param>
    public void takeDamage(float damage)
    {
        _currentLife = Mathf.Clamp(_currentLife - damage, 0, _stats.maxLife);
    }

    public void heal(float life)
    {
        _currentLife = Mathf.Clamp(_currentLife + life, 0, _stats.maxLife);
    }

    /// <summary>
    /// Add a stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public bool addStat(Stat stat)
    {
        return _stats.addStat(stat);
    }

    /// <summary>
    ///  remove a stat with the source name
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    public bool removeStat(string sourceName)
    {
        return _stats.removeStat(sourceName);
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
