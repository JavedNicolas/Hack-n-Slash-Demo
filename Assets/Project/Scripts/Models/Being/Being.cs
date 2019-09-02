﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Being : Interactable
{
    #region being attributs

    [Header("Health")]
    private float _currentLife;
    public float currentLife { get { return _currentLife; } }
    [SerializeField] private float _baseLife;
    public float baseLife { get { return _baseLife; } }
    private float _shield;
    public float shield { get { return _shield; } }

    [Header("Damage")]
    private float _bonusAttackSpeed;
    public float bonusAttackSpeed { get { return _bonusAttackSpeed; } }

    private float _bonusCastSpeed;
    public float bonusCastSpeed { get { return _bonusCastSpeed; } }

    private float _attackRange;
    public float attackRange { get { return _attackRange; } }

    [Header("Stats")]
    private int _strength;
    public int strength { get { return _strength; } }

    [Header("Projectiles")]
    private float _projectileSpeed;
    public float projectileSpeed { get { return _projectileSpeed; } }

    [Header("Skills")]
    [SerializeField] private List<Ability> _ability = new List<Ability>();
    public List<Ability> ability { get { return _ability; } }

    private BasicAttack _basicAttack;
    public BasicAttack basicAttack{ get { return _basicAttack;}}

    [Header("Game Object")]
    [SerializeField] private GameObject _prefab;
    public GameObject prefab {  get { return _prefab; } }

    [Header("Misc")]
    private float _movementSpeedBonus;
    public float movementSpeedBonus {  get { return _movementSpeedBonus; } }

    #endregion

    #region init
    public Being(string name, float baseLife, float shield, float aSPD, float attackRange, int strength, List<Ability> abilities, float movementSpeedPercentage, GameObject prefab, float projectileSpeed)
    {
        this.name = name;
        this._baseLife = baseLife;
        this._shield = shield;
        _bonusAttackSpeed = aSPD;
        this._attackRange = attackRange;
        this._strength = strength;
        this._ability = abilities;
        this._movementSpeedBonus = movementSpeedPercentage;
        this._prefab = prefab;
        this._projectileSpeed = projectileSpeed;
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
    public float getASPD()
    {
        return BeingConstant.baseASPD + (BeingConstant.baseASPD * (bonusAttackSpeed / 100));
    }

    public float getCastPerSecond()
    {
        return BeingConstant.baseCastperSecond + (BeingConstant.baseCastperSecond * (bonusCastSpeed / 100));
    }

    public float getMovementSpeed()
    {
        return BeingConstant.baseMoveSpeed + (BeingConstant.baseMoveSpeed * (_movementSpeedBonus / 100));
    }

    public float getCurrentMaxLife()
    {
        return _baseLife;
    }

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
