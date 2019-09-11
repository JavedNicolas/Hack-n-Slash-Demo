using UnityEngine;
using System.Collections;

public class PlayerStats : BeingStats
{
    [SerializeField] private float _maxMana;
    public float maxMana { get => Mathf.FloorToInt(getBuffedValue(_maxMana, StatType.Mana)); }

    [SerializeField] private int _currentJobLevel;
    public int currentJobLevel { get => _currentJobLevel; }

    public PlayerStats(PlayerStats stats) : base(stats)
    {
        _maxMana = stats.maxMana;
        _currentJobLevel = stats.currentJobLevel;
    }

    public PlayerStats(float maxLife,float maxMana,  float attackSpeed, float castSpeed, float attackRange, float movementSpeed) : 
        base(maxLife, attackSpeed, castSpeed, attackRange, movementSpeed)
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
