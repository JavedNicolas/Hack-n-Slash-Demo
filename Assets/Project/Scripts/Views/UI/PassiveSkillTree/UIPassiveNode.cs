using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NodeStyle { Allocated, Unallocated, Locked }

public class UIPassiveNode : BaseUI, IPopUpOnHovering
{
    [SerializeField] string _GUID = "";

    [Header("Node position and links")]
    int currentLevel = 0;
    [SerializeField] bool _isBase = false;
    [SerializeField] bool _isAllocated = false;
    [SerializeField] Vector3 _position;
    [SerializeField] List<UIPassiveNode> _connectedNodes = new List<UIPassiveNode>();

    [Header("Levels")]
    [SerializeField] UINodeLevelDisplayer _levelHolder;

    [Header("Node")]
    [SerializeField] PassiveNode _node;

    [Header("Links")]
    [SerializeField] Transform _linksHolder;
    [SerializeField] LineRenderer _lineRendererPrefab;

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
    public Vector3 position => _position; 
    public List<UIPassiveNode> connectedNodes => _connectedNodes; 
    public PassiveNode node  => _node;
    public string GUID => _GUID;

    private void Update()
    {
        if (!connectedNodes.Exists(x => x.isAllocated) && !isBase)
            setStyle(NodeStyle.Locked);
        else
            setStyle(_isAllocated ? NodeStyle.Allocated : NodeStyle.Unallocated);
    }

    /// <summary>
    /// Set attributs from model
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    /// <param name="isBase"></param>
    public void setNodeFromModel(string id, Vector3 position, bool isBase, PassiveNode node)
    {
        this._GUID = id;
        this._position = position;
        this._isBase = isBase;
        this._node = node;
    }

    public void initNode()
    {
        if(_GUID == "")
            generateID();

        name = _node.name;
        _levelHolder?.initDisplayer(Random.Range(1, 4));
    }

    /// <summary>
    /// save the current position
    /// </summary>
    public void saveCurrentPosition()
    {
        _position = transform.localPosition;
    }

    /// <summary>
    /// Move the node to the saved position
    /// </summary>
    public void moveNodeToSavedPosition()
    {
        transform.localPosition = _position;
    }

    public void drawLinks()
    {
        foreach (UIPassiveNode connectionNode in connectedNodes)
        {
            // set the line position
            List<Vector3> positions = new List<Vector3>();

            positions.Add(transform.position);
            positions.Add(connectionNode.transform.position);
            // create and set lineRenderer
            LineRenderer _defaultLineRenderer = Instantiate(_lineRendererPrefab);

            Color lineColor = getLineColor(connectionNode);

            _defaultLineRenderer.transform.SetParent(_linksHolder);
            _defaultLineRenderer.positionCount = positions.Count;
            _defaultLineRenderer.startColor = lineColor;
            _defaultLineRenderer.endColor = lineColor;

            _defaultLineRenderer.SetPositions(positions.ToArray());
        }
    }

    /// <summary>
    /// Update the color of the link based on the allocation of each node
    /// </summary>
    /// <param name="connectedNode">The node connected with the link to modify</param>
    void updateConnectedLinkColor()
    {
        foreach (UIPassiveNode connectedNode in connectedNodes)
        {
            LineRenderer[] lineRenderers = _linksHolder.GetComponentsInChildren<LineRenderer>();

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                if (lineRenderers[i].GetPosition(0) == connectedNode.transform.position || lineRenderers[i].GetPosition(1) == connectedNode.transform.position)
                {
                    Color lineColor = getLineColor(connectedNode);
                    lineRenderers[i].startColor = lineColor;
                    lineRenderers[i].endColor = lineColor;
                    break;
                }
            }
        } 
    }

    #region graphical change
    /// <summary>
    /// Set the style of the node based on his allocation
    /// </summary>
    /// <param name="nodeStyle"></param>
    void setStyle(NodeStyle nodeStyle) 
    {
        switch (nodeStyle)
        {
            case NodeStyle.Allocated: setAllocatedStyle(); break;
            case NodeStyle.Unallocated: setUnallocatedStyle(); break;
            case NodeStyle.Locked: setLockedStyle(); break;
        }
    }

    void setAllocatedStyle()
    {
        GetComponent<Image>().color = Color.blue;
    }

    void setUnallocatedStyle()
    {
        GetComponent<Image>().color = Color.white;
        _levelHolder?.displayLockLevel(false);
    }

    void setLockedStyle()
    {
        GetComponent<Image>().color = Color.gray;
        _levelHolder?.displayLockLevel(true);
    }

    /// <summary>
    /// Get the color on link line based on the allocation
    /// </summary>
    /// <param name="connectedNode"></param>
    /// <returns></returns>
    Color getLineColor(UIPassiveNode connectedNode)
    {
        if (_isAllocated && connectedNode.isAllocated)
            return Color.blue;
        if ((_isAllocated && !connectedNode.isAllocated) || (!_isAllocated && connectedNode.isAllocated))
            return Color.white;

        return Color.gray;
    }
    #endregion

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
            if (connectedNodes.Exists(x => x._isAllocated == true) && (!_isAllocated || currentLevel < getMaxLevel()))
                isAllocated = true;
        }
    }

    protected override void rightClickOnUI()
    {
        if (!_isBase)
        {
            if (currentLevel >= 1)
            {
                currentLevel--;
                _levelHolder.updateLevel(currentLevel);
            }
            
            if(currentLevel == 0)
            {
                isAllocated = false;
            }
        }
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenAllocated()
    {
        setStyle(NodeStyle.Allocated);

        updateConnectedLinkColor();
        foreach (UIPassiveNode connectedNode in connectedNodes)
            connectedNode.updateConnectedLinkColor();

        if (!isBase && currentLevel < getMaxLevel())
        {
            currentLevel++;
            _levelHolder.updateLevel(currentLevel);
        }
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenUnallocated()
    {
        setStyle(NodeStyle.Unallocated);

        updateConnectedLinkColor();
        foreach (UIPassiveNode connectedNode in connectedNodes)
            connectedNode.updateConnectedLinkColor();

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

    protected override void dragging(PointerEventData eventData){}

    protected override void dragginEnd(PointerEventData eventData){ }
    #endregion

    public void setNode(PassiveNode node)
    {
        this._node = node;
        name = node.name;
    }

    public void generateID()
    {
        _GUID = System.Guid.NewGuid().ToString();
    }

    public int getMaxLevel() { return  _levelHolder._levelDisplayers.Count == 0 ? 1 :_levelHolder._levelDisplayers.Count; }
    /// <summary>
    /// Get the position with 1 on the z. (used for the lines)
    /// </summary>
    /// <returns></returns>
    public Vector3 getWorldPosition() { Vector3 alteredPosition = _position; alteredPosition.z = 1; return alteredPosition; }
}
