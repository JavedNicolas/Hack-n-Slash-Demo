using UnityEngine;
using System.Collections;

[System.Serializable]
public class AllocatedNodeInfo
{
    [SerializeField] public string nodeGUID;
    [SerializeField] public int currentLevel;

    public AllocatedNodeInfo(string nodeGUID, int currentLevel)
    {
        this.nodeGUID = nodeGUID;
        this.currentLevel = currentLevel;
    }
}
