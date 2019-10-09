using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NodeStyle { Allocated, Unallocated, Locked }

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Outline))]
public class UIPassiveNode : BaseUI, IPopUpOnHovering, IDescribable
{
    [SerializeField] string _GUID = "";
    [SerializeField] Player _player;

    [Header("Node position")]
    int _currentLevel = 0;
    [SerializeField] bool _isBase = false;
    [SerializeField] Vector3 _position;
    [SerializeField] List<UIPassiveNode> _connectedNodes = new List<UIPassiveNode>();

    [Header("Levels")]
    [SerializeField] UINodeLevelDisplayer _levelHolder;

    [Header("Node")]
    [SerializeField] Image _iconImage;
    [HideInInspector][SerializeField] PassiveNode _node;

    [Header("Node Color")]
    [SerializeField] Color _allocatedNodeColor;
    [SerializeField] Color _lockedNodeColor;
    [SerializeField] Color _unallocatedNodeColor;

    public bool isBase => _isBase;
    public Vector3 position => _position; 
    public List<UIPassiveNode> connectedNodes => _connectedNodes; 
    public PassiveNode node  => _node;
    public string GUID => _GUID;
    public int currentLevel => Mathf.Clamp(_currentLevel, 0, node.maxLevel); 

    #region allocation delegate
    public delegate void NodeAllocationModified(bool isAllocated, UIPassiveNode node);
    public NodeAllocationModified allocationModified;
    #endregion

    private void Update()
    {
        if(_player != null)
            updateStyle();
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

    public void setPlayer(Player player)
    {
        this._player = player;
    }

    public void initNode()
    {
        if(_GUID == "")
            generateID();

        name = _node.name;
        _iconImage.sprite = node?.icon;
        _levelHolder?.initDisplayer(node.maxLevel, _allocatedNodeColor, _lockedNodeColor, _unallocatedNodeColor);
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

    #region graphical change
    /// <summary>
    /// Set the style of the node based on his allocation
    /// </summary>
    /// <param name="nodeStyle"></param>
    public void updateStyle() 
    {
        if (isAllocated())
        {
            GetComponent<Outline>().effectColor = _allocatedNodeColor;
            if (_player?.getRemainingPassivepoint() == 0)
                _levelHolder?.displayLockLevel(true, currentLevel);
            else
                _levelHolder?.displayLockLevel(false, currentLevel);
        }
        else if (!isAllocated() && connectedNodes.Exists(x => x.isAllocated()) && _player.getRemainingPassivepoint() > 0)
        {
            GetComponent<Outline>().effectColor = _unallocatedNodeColor;
            _levelHolder?.displayLockLevel(false, currentLevel) ;
        }
        else if (!isAllocated() && !connectedNodes.Exists(x => x.isAllocated()) || _player.getRemainingPassivepoint() == 0)
        {
            GetComponent<Outline>().effectColor = _lockedNodeColor;
            _levelHolder?.displayLockLevel(true);
        }
    }
    #endregion

    #region describable interface

    public string getName()
    {
        return name;
    }

    public string getDescription(Being owner)
    {
        string description = "";
        foreach(PassiveNodeStat passiveNodeStat in node.stats)
        {
            // if the node is allocated then display current effect
            // else display all possible stats
            if(isAllocated())
            {
                Stat stat = passiveNodeStat.getStatForLevel(Mathf.Clamp(currentLevel, 1, node.maxLevel));
                description += StatDescription.getStatDescription(stat.value, stat.statType, stat.bonusType);
            }
            else
            {
                for (int i = 0; i < passiveNodeStat.levelValue.Count; i++)
                {
                    Stat stat = passiveNodeStat.getStatForLevel(Mathf.Clamp(i + 1, 1, node.maxLevel));
                    description += "(Lv : " + (i + 1) + ") " + StatDescription.getStatDescription(stat.value, stat.statType, stat.bonusType) + "\n";
                }
            }
         
        }

        return description;
    }


    public string getSmallDescription(Being owner)
    {
        string description = "";
        foreach (PassiveNodeStat passiveNodeStat in node.stats)
        {
            for(int i = 0; i < passiveNodeStat.levelValue.Count; i++)
            {
                // only display the relevante bonus
                if(currentLevel > 0 && i + 1!= currentLevel)
                {
                    Stat stat = passiveNodeStat.getStatForLevel(Mathf.Clamp(i + 1, 1, node.maxLevel));
                    description += "(Lv : " + (i+1) + ") " + StatDescription.getStatDescription(stat.value, stat.statType, stat.bonusType) + "\n";
                }
            }
        }

        return description;
    }
    #endregion

    #region hovering
    public void displayPopUp(bool display)
    {
        if(!isBase)
            GameUI.instance.displayDescription(display, this, this, true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        displayPopUp(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayPopUp(false);
    }
    #endregion

    #region click handling
    protected override void leftClickOnUI()
    {
        if (!_isBase)
        {
            if (connectedNodes.Exists(x => x.isAllocated() == true) && (!isAllocated() || currentLevel < getMaxLevel()) && _player.getRemainingPassivepoint() > 0)
                hasBeenAllocated();
        }
    }

    protected override void rightClickOnUI()
    {
        if (!_isBase)
        {
            if (_connectedNodes.FindAll(x => x.isAllocated()).Count > 1)
                // check if each allocated connected node are link to a base
                foreach (UIPassiveNode connectNode in connectedNodes)
                {
                    if (!checkRoadToBase(connectNode))
                    {
                        return;
                    }
                }

            hasBeenUnallocated();
        }
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenAllocated()
    {
        if (!isBase)
        {
            _currentLevel++;
            _levelHolder.updateLevel(currentLevel);
        }

        GameUI.instance.updateDisplayPopUP(this, this, true);
        allocationModified?.Invoke(true, this);
    }

    /// <summary>
    /// Allocate or unallocated the node
    /// </summary>
    /// <param name="isAllocated"></param>
    void hasBeenUnallocated()
    {
        if (currentLevel > 0)
        {
            _currentLevel--;
            _levelHolder.updateLevel(currentLevel);
        }

        updateStyle();

        GameUI.instance.updateDisplayPopUP(this, this, true);
        allocationModified?.Invoke(false, this);
    }

    public void setLevel(int level)
    {
        _currentLevel = level;
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
                if (nodeToCheck.connectedNodes[i].isAllocated() && nodeToCheck.connectedNodes[i].isBase)
                    return true;
                // else if the node is allocated, not the base and has no been checked then add it
                // to the list of node to check
                else if (nodeToCheck.connectedNodes[i].isAllocated() && !nodeToCheck.connectedNodes[i].isBase && !nodesChecked.Contains(nodeToCheck.connectedNodes[i]))
                    nodesToCheck.Add(nodeToCheck.connectedNodes[i]);
            }
        }

        return false;
    }

    protected override void dragging(PointerEventData eventData){}

    protected override void dragginEnd(PointerEventData eventData){ }
    #endregion

    /// <summary>
    /// Allocate a node without checking if it's possible (used to load the allocation of a player)
    /// </summary>
    /// <param name="allocate"></param>
    public void allocatedWithoutChecking(int currentLevel)
    {
        this._currentLevel = currentLevel;
        _levelHolder?.updateLevel(currentLevel);
    }

    public void setNode(PassiveNode node)
    {
        this._node = node;
        name = node.name;
        this._levelHolder?.initDisplayer(node.maxLevel, _allocatedNodeColor, _lockedNodeColor, _unallocatedNodeColor);
    }

    public void generateID()
    {
        _GUID = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Get the position of the center of the item even if the pivot has been modified
    /// </summary>
    /// <returns></returns>
    public Vector3 getCenterPosition()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float x = transform.position.x - rect.sizeDelta.x * (rect.pivot.x - 0.5f);
        float y = transform.position.y - rect.sizeDelta.y * (rect.pivot.y - 0.5f);

        return new Vector3(x, y, 1);
    }

    public bool isAllocated() { return _currentLevel > 0 ? true : false; }
    public int getMaxLevel() { return  node.maxLevel; }
}
