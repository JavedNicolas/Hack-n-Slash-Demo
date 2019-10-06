using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIPassiveNodeModel 
{
    [SerializeField] public string GUID;
    [SerializeField] public Vector3 position;
    [SerializeField] public List<string> connectedNodesGUID;
    [SerializeField] public bool isBase;
    [SerializeField] public PassiveNodeModel node;

    public UIPassiveNodeModel(UIPassiveNode uiNode, ResourcesList resourcesList)
    {
        this.GUID = uiNode.GUID;
        this.position = uiNode.position;
        this.connectedNodesGUID = new List<string>();
        foreach (UIPassiveNode connectedNode in uiNode.connectedNodes)
            this.connectedNodesGUID.Add(connectedNode.GUID);
        this.isBase = uiNode.isBase;
        this.node = new PassiveNodeModel(uiNode.node, resourcesList);
    }   

    public UIPassiveNode modelToUIPassiveNode(UIPassiveNode nodeToSet, ResourcesList resourcesList)
    {
        PassiveNode passiveNode = node?.modelToPassiveNode(resourcesList); 
        nodeToSet.setNodeFromModel(GUID, position, isBase, passiveNode);
        return nodeToSet;
    }

}

