using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PassiveNode 
{
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] public List<Stat> stats = new List<Stat>();
    [SerializeField] public int maxLevel;

    public PassiveNode() { maxLevel = 1; }

    public PassiveNode(string name, Sprite icon, List<Stat> nodeStats, int maxLevel)
    {
        this.name = name;
        this.icon = icon;
        this.stats = nodeStats;
        this.maxLevel = maxLevel == 0 ? 1 : maxLevel;
    }
}
