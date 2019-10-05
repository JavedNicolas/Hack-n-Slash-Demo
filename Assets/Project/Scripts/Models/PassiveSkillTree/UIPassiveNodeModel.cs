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

    public UIPassiveNodeModel(UIPassiveNode node)
    {
        this.GUID = node.GUID;
        this.position = node.position;
        this.connectedNodesGUID = new List<string>();
        foreach (UIPassiveNode connectedNode in node.connectedNodes)
            this.connectedNodesGUID.Add(connectedNode.GUID);
        this.isBase = node.isBase;
    }   

    public UIPassiveNode modelToUIPassiveNode(UIPassiveNode nodeToSet)
    {
        nodeToSet.setNodeFromModel(GUID, position, isBase);
        return nodeToSet;
    }

}

