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
    [SerializeField] public BeingStats stats = new BeingStats();

    [Header("Abilities")]
    [SerializeField] List<Ability> _abilities = new List<Ability>();
    [SerializeField] public List<Ability> abilities { get { return _abilities; } }

    [SerializeField] public List<int> abilityIDs = new List<int>();

    private BasicAttack _basicAttack;
    public BasicAttack basicAttack{ get { return _basicAttack;}}

    [Header("Game Object")]
    [SerializeField] private GameObject _prefab;
    public GameObject prefab {  get { return _prefab; } }

    #endregion

    #region init
    public Being() { }

    public Being(string name, float baseLife,  float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed, List<int> abilityIndexes, GameObject prefab)
    {
        this.name = name;
        this.stats = new BeingStats(baseLife, baseASPD, baseCastSpeed, baseAttackRange, baseMovementSpeed);
        this.abilityIDs = abilityIndexes;
        this._prefab = prefab;
        this._basicAttack = new BasicAttack();
        this._currentLife = stats.maxLife;
        setAbilities();
    }

    void setAbilities()
    {
        for(int i = 0; i < abilityIDs.Count; i++)
            addAbility(abilityIDs[i]);
    }

    #endregion

    /// <summary>
    /// Apply damage to the being
    /// </summary>
    /// <param name="damage">the damage to apply</param>
    public void takeDamage(float damage)
    {
        _currentLife = Mathf.Clamp(_currentLife - damage, 0, stats.maxLife);
    }

    public void heal(float life)
    {
        _currentLife = Mathf.Clamp(_currentLife + life, 0, stats.maxLife);
    }

    /// <summary>
    /// Add an ability to the being ability list
    /// </summary>
    /// <param name="abilityID">The databaseId of the ability</param>
    /// <returns></returns>
    public bool addAbility(int abilityID, bool permantlyAdd = false) {
        if (abilities.Exists(x => x.databaseID == abilityID))
            return false;

        if (permantlyAdd)
            abilityIDs.Add(abilityID);

        if (GameManager.instance == null)
            return false;

        _abilities.Add(GameManager.instance.abilityDatabase.getElementWithDBID(abilityID));
        return true;
    }

    /// <summary>
    /// Add a stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public bool addStat(Stat stat)
    {
        return stats.addStat(stat);
    }

    /// <summary>
    ///  remove a stat with the source name
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    public bool removeStat(string sourceName)
    {
        return stats.removeStat(sourceName);
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
