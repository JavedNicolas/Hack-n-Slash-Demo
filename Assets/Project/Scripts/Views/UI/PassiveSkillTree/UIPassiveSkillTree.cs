using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIPassiveSkillTree : MonoBehaviour
{
    [Header("Node links attributs")]
    [SerializeField][Range(0, 10)] float _lineWidth = 2;
    [SerializeField] Color _lineColor;
    [SerializeField] Material _lineMaterial;

    [SerializeField] List<UIPassiveNode> nodes = new List<UIPassiveNode>();

    private void Start()
    {
        updateNodes();
    }

    /// <summary>
    /// Update the nodes with the current passive node childs
    /// </summary>
    public void updateNodes()
    {
        nodes = new List<UIPassiveNode>();
        nodes = GetComponentsInChildren<UIPassiveNode>().ToList();
    }


    /// <summary>
    /// Draw all the nodes links
    /// </summary>
    public void drawNodeLink()
    {
        for(int i =0; i < nodes.Count; i++)
        {
            nodes[i].drawLinks(_lineWidth, _lineColor, _lineMaterial);
        }
    }
}
