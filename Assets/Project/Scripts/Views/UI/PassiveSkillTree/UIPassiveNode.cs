using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class UIPassiveNode : BaseUI, IPopUpOnHovering
{
    [Header("Link drawing")]
    [SerializeField] public LineRenderer lineRenderer;

    [Header("Node position and links")]
    [SerializeField] bool _isBase = false;
    [SerializeField] bool _isAllocated = false;
    [SerializeField] Vector3 _position;
    [SerializeField] List<UIPassiveNode> _connectedNodes = new List<UIPassiveNode>();

    [Header("Node")]
    [SerializeField] PassiveNode _node;

    public bool isAllocated
    {
        get { return _isAllocated; }
        set { 
            _isAllocated = value;
            if (_isAllocated)
                hasBeenAllocated();
            else
                hasBeenUnallocated();
        }
    }
    public bool isBase => _isBase;
    public Vector3 position  => _position; 
    public List<UIPassiveNode> connectedNodes => _connectedNodes; 
    public PassiveNode node  => _node;

    public void setPosition()
    {
        _position = transform.position;
    }

    /// <summary>
    /// Draw the links with the linked nodes
    /// </summary>
    /// <param name="lineWidth">The with of the line</param>
    /// <param name="lineColor">The color of the line</param>
    public void drawLinks(float lineWidth, Color lineColor, Material lineMaterial)
    {
        List<Vector3> positions = new List<Vector3>();

        // set the line renderer
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = connectedNodes.Count * 2;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;

        foreach (UIPassiveNode connectionNode in _connectedNodes)
        {
            positions.Add(position);
            positions.Add(connectionNode.position);
        }

        lineRenderer.SetPositions(positions.ToArray());
    }

    #region hovering
    public void displayPopUp(bool display)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
    #endregion

    #region click handling
    protected override void leftClickOnUI()
    {
        if (!_isBase)
        {
            if (connectedNodes.Exists(x => x._isAllocated == true) && !_isAllocated)
                isAllocated = true;
            else if (_isAllocated)
                isAllocated = false;
        }
       
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenAllocated()
    {
        GetComponent<Image>().color = Color.blue;
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenUnallocated()
    {
        GetComponent<Image>().color = Color.white;

        if (_connectedNodes.FindAll(x => x.isAllocated).Count > 1)
            // check if each allocated connected node are link to a base
            foreach (UIPassiveNode connectNode in connectedNodes)
            {
                if (!checkRoadToBase(connectNode))
                {
                    isAllocated = true;
                    return;
                }
            }
    }

    /// <summary>
    /// Check if a node path can be traced back to base
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    bool checkRoadToBase(UIPassiveNode node)
    {
        // Node to check
        List<UIPassiveNode> nodesToCheck = new List<UIPassiveNode>();
        // Node already checked : avoid to recheck branch we already checked
        List<UIPassiveNode> nodesChecked = new List<UIPassiveNode>(); 

        // The node currently being check
        UIPassiveNode nodeToCheck = node;

        while (nodesToCheck.Count > 0)
        {
            // if there is a node to check next then assign it to nodeToCheck
            // and remove it from the list of node to check
            if (connectedNodes.Count > 0)
            {
                nodeToCheck = nodesToCheck[0];
                nodesToCheck.RemoveAt(0);
                nodesChecked.Add(nodeToCheck);
            }

            // search the allocated base in the connectedNodes
            for (int i = 0; i < nodeToCheck.connectedNodes.Count; i++)
            {
                // if there is the base then return true
                if (nodeToCheck.connectedNodes[i].isAllocated && nodeToCheck.connectedNodes[i].isBase)
                    return true;
                // else if the node is allocated, not the base and has no been checked then add it
                // to the list of node to check
                else if (nodeToCheck.connectedNodes[i].isAllocated && !nodeToCheck.connectedNodes[i].isBase && !nodesChecked.Contains(nodeToCheck.connectedNodes[i]))
                    nodesToCheck.Add(nodeToCheck.connectedNodes[i]);
            }
        }

        return false;
    }

    protected override void rightClickOnUI(){}

    protected override void dragging(PointerEventData eventData){}

    protected override void dragginEnd(PointerEventData eventData){ }
    #endregion


}
