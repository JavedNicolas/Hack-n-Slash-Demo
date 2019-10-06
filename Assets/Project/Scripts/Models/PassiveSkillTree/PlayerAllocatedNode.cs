using UnityEngine;
using System.Collections;

[System.Serializable]
public struct PlayerAllocatedNode
{
    [SerializeField] public string nodeGUID;
    [SerializeField] public int currentLevel;

    public PlayerAllocatedNode(string nodeGUID, int currentLevel)
    {
        this.nodeGUID = nodeGUID;
        this.currentLevel = currentLevel;
    }
}
