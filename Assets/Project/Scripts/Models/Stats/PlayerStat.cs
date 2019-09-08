using UnityEngine;
using System.Collections;

public class PlayerStat : BeingStats
{
    [SerializeField] private float _maxMana;
    public float maxMana { get => Mathf.FloorToInt(getBuffedValue(_maxMana, StatType.Mana)); }

    [SerializeField] private int _currentJobLevel;
    public int currentJobLevel { get => _currentJobLevel; }


    public PlayerStat(float maxLife,float maxMana,  float attackSpeed, float castSpeed, float attackRange, float movementSpeed, int currentLevel) : 
        base(maxLife, attackSpeed, castSpeed, attackRange, movementSpeed, currentLevel)
    {
        this._maxMana = maxMana;
        this._currentJobLevel = 0;
    }

    public void levelUp()
    {
        _currentLevel++;
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
