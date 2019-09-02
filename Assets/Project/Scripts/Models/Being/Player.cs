using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
    [Header("Inventory")]
    private Inventory _inventory = new Inventory();
    public Inventory inventory { get { return _inventory; } }

    [SerializeField] private float _baseMana;
    public float baseMana { get => _baseMana; }

    [SerializeField] private float _currentMana;
    public float currentMana { get => _currentMana; }

    public Player(string name, float baseLife, float baseMana, float shield, float aSPD, float attackRange, int strength, List<Ability> skills, float movementSpeedPercentage, GameObject prefab, float projectileSpeed) : base(name, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage, prefab, projectileSpeed)
    {
        this._baseMana = baseMana;
        this._currentMana = getCurrentMaxMana();
    }

    public float getCurrentMaxMana()
    {
        return _baseMana;
    }

    public void spendMana(float mana)
    {
        _currentMana = Mathf.Clamp(_currentMana - mana, 0, getCurrentMaxMana());
    }
}
