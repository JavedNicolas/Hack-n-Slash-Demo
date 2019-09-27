using System;
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
    public GameObject model {  get { return _prefab; } }

    #endregion

    #region init
    public Being() { }

    public Being(string name, float baseLife,  float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed, List<int> abilityIndexes, GameObject prefab, int currentLevel = 1)
    {
        this.name = name;
        this.stats = new BeingStats(baseLife, baseASPD, baseCastSpeed, baseAttackRange, baseMovementSpeed, currentLevel);
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
    public void addAbility(int abilityID, bool permantlyAdd = false) {
        if (abilities.Count > 0 && abilities.Exists(x => x.databaseID == abilityID))
            return;

        if (permantlyAdd)
            abilityIDs.Add(abilityID);

        if (GameManager.instance == null)
            return;

        Ability AbilityBuffer = GameManager.instance.abilityDatabase.getAbilityFromDatabaseID(abilityID, GameManager.instance.resourcesList);
        // Make a new instance of this ability 
        Ability ability = (Ability)Activator.CreateInstance(AbilityBuffer.GetType());
        ability.copyAttributs(AbilityBuffer);

        _abilities.Add(ability);
        return;
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
