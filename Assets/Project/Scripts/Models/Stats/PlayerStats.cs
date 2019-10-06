using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : BeingStats
{
    [SerializeField] private float _maxMana;
    public float maxMana { get => Mathf.FloorToInt(getBuffedValue(_maxMana, StatType.Mana)); }

    [SerializeField] private int _currentJobLevel;
    public int currentJobLevel { get => _currentJobLevel; }

    public PlayerStats(int currentLevel = 1) : base(currentLevel) { }

    public PlayerStats(PlayerStats stats) : base(stats)
    {
        _stats = new List<Stat>();
        _maxMana = stats.maxMana;
        _currentJobLevel = stats.currentJobLevel;
    }

    public PlayerStats(float maxLife,float maxMana,  float attackSpeed, float castSpeed, float attackRange, float movementSpeed, int currentLevel = 1) : 
        base(maxLife, attackSpeed, castSpeed, attackRange, movementSpeed, currentLevel)
    {
        this._maxMana = maxMana;
        this._currentJobLevel = 0;
    }

    protected override float getInflencedFactor(Stat stat)
    {
        switch (stat.isInfluencedBy)
        {
            case StatInfluencedBy.JobLevel: return _currentJobLevel;
            case StatInfluencedBy.MaxMana: return _maxMana;
            default: return base.getInflencedFactor(stat);
        }
    }
}
