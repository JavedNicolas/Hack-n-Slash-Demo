using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PassiveNode 
{
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] public List<Stat> nodeStats = new List<Stat>();
    [SerializeField] public int maxLevel = 2;

    public PassiveNode() { }

    public PassiveNode(string name, Sprite icon, List<Stat> nodeStats, int maxLevel)
    {
        this.name = name;
        this.icon = icon;
        this.nodeStats = nodeStats;
        this.maxLevel = maxLevel;
    }
}
