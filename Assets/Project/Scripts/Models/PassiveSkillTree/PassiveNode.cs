using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PassiveNode 
{
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] public List<PassiveNodeStat> stats = new List<PassiveNodeStat>();
    [SerializeField] public int maxLevel;

    public PassiveNode() { maxLevel = 1; }

    public PassiveNode(string name, Sprite icon, List<PassiveNodeStat> nodeStats, int maxLevel)
    {
        this.name = name;
        this.icon = icon;
        this.stats = nodeStats;
        this.maxLevel = maxLevel == 0 ? 1 : maxLevel;
    }
}

[System.Serializable]
public class PassiveNodeStat
{
    [SerializeField] public Stat stat = new Stat();
    [SerializeField] public List<float> levelValue = new List<float>();

    public Stat getStatForLevel(int level)
    {
        return new Stat(stat.statType, stat.bonusType, levelValue[level-1], stat.sourceName, stat.isInfluencedBy, stat.influencedEvery, stat.isSpecificTo);
    }
}
