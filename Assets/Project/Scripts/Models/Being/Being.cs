using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Being
{
    #region being attributs
    [Header("Name")]
    private string _name;
    public string name { get { return _name; } }

    [Header("Health")]
    private float _currentLife;
    public float currentLife {  get { return _currentLife; } }
    private float _baseLife;
    public float baseLife {  get { return _baseLife; } }
    private float _shield;
    public float shield {  get { return _shield; } }

    [Header("Damage")]
    private float _bonusAttackSpeed;
    public float bonusAttackSpeed { get { return _bonusAttackSpeed; } }

    private float _bonusCastSpeed;
    public float bonusCastSpeed { get { return _bonusCastSpeed; } }

    private float _attackRange;
    public float attackRange { get { return _attackRange; } }

    [Header("Stats")]
    private int _strength;
    public int strength {  get { return _strength; } }

    [Header("Projectiles")]
    private float _projectileSpeed;
    public float projectileSpeed {  get { return _projectileSpeed; } }

    [Header("Skills")]
    private List<Skill> _skills = new List<Skill>();
    public List<Skill> skills {  get { return _skills; } }

    [Header("Game Object")]
    private GameObject _prefab;
    public GameObject prefab {  get { return _prefab; } }

    [Header("Misc")]
    private float _movementSpeedBonus = 100;
    public float movementSpeedBonus {  get { return _movementSpeedBonus; } }

    #endregion

    #region init
    public Being(string name, float currentLife, float baseLife, float shield, float aSPD, float attackRange, int strength, List<Skill> skills, float movementSpeedPercentage, GameObject prefab, float projectileSpeed)
    {
        this._name = name;
        this._currentLife = currentLife;
        this._baseLife = baseLife;
        this._shield = shield;
        _bonusAttackSpeed = aSPD;
        this._attackRange = attackRange;
        this._strength = strength;
        this._skills = skills;
        this._movementSpeedBonus = movementSpeedPercentage;
        this._prefab = prefab;
        this._projectileSpeed = projectileSpeed;

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



}
