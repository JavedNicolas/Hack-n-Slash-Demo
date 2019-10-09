using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PassiveNodeModel
{
    [SerializeField] public string name;
    [SerializeField] public string iconGUID;
    [SerializeField] public List<PassiveNodeStat> nodeStats = new List<PassiveNodeStat>();
    [SerializeField] public int maxLevel;

    public PassiveNodeModel(PassiveNode passiveNode, ResourcesList resourcesList)
    {
        this.name = passiveNode.name;
        this.iconGUID = resourcesList.getGUIDFor(passiveNode.icon);
        this.nodeStats = passiveNode.stats;
        this.maxLevel = passiveNode.maxLevel;
    }

    public PassiveNode modelToPassiveNode(ResourcesList resourcesList)
    {
        Sprite icon = (Sprite)resourcesList.getObject<Sprite>(iconGUID);
        return new PassiveNode(name, icon, nodeStats, maxLevel);

    }
}
