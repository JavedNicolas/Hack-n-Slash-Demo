using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine.EventSystems;

public class UIPassiveTree : BaseUI
{
    public static UIPassiveTree instance;

    [Header("Skill tree saving")]
    [SerializeField] Object _passiveTreeSaveFile;
    [SerializeField] ResourcesList resourcesList;

    [Header("Skill Tree Elements")]
    [SerializeField] GameObject _baseContainer;
    [SerializeField] GameObject _nodeContainer;

    [Header("Node links attributs")]
    [SerializeField] GameObject basePrefab;
    [SerializeField] GameObject nodePrefab;

    [SerializeField] List<UIPassiveNode> nodes = new List<UIPassiveNode>();
    [SerializeField] Player _player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        loadPassiveTreeFromJson();
        initNodes();
        moveNodesToSavedPosition();
        drawDefaultNodeLink();
        initAllocation(GameManager.instance.getPlayer());
    }

    private void Update()
    {
        zoom();
    }

    public void initAllocation(Player player)
    {
        _player = player;
        
        foreach (PlayerAllocatedNode allocatedNodeInfo in _player.allocatedNodesInfo)
        {
            UIPassiveNode node = nodes.Find(x => x.GUID == allocatedNodeInfo.nodeGUID);
            if(node != null)
            {
                node.allocatedWithoutChecking(true, allocatedNodeInfo.currentLevel);
                foreach (Stat nodeStat in node.node.stats)
                    _player.stats.addStat(nodeStat);
            }
            else
            {
                player.allocatedNodesInfo.Remove(allocatedNodeInfo);
            }
        }

        foreach(UIPassiveNode node in nodes){
            if (node.isAllocated && !node.connectedNodes.Exists(x => x.isAllocated))
            {
                node.isAllocated = false;
            }
        }
    }

    /// <summary>
    /// remove a node from the player
    /// </summary>
    /// <param name="node"></param>
    void addNodeToPlayer(UIPassiveNode node)
    {
        if (!_player.allocatedNodesInfo.Exists(x => x.nodeGUID == node.GUID))
        {
            _player.allocatedNodesInfo.Add(new PlayerAllocatedNode(node.GUID, node.currentLevel));
            foreach (Stat nodeStat in node.node.stats)
                _player.stats.addStat(nodeStat);
        }
    }

    /// <summary>
    /// remove a node from the player
    /// </summary>
    /// <param name="node"></param>
    void removeNodeFromPlayer(UIPassiveNode node)
    {
        _player.allocatedNodesInfo.RemoveAll(x => x.nodeGUID == node.GUID);
        _player.stats.removeStat(node.name);
    }

    /// <summary>
    /// Update the nodes with the current passive node childs
    /// </summary>
    public void getChildNodes()
    {
        nodes = new List<UIPassiveNode>();
        nodes = GetComponentsInChildren<UIPassiveNode>().ToList();
    }

    /// <summary>
    /// init nodes in the passive list
    /// </summary>
    public void initNodes()
    {
        for (int i = 0; i < nodes.Count; i++) { 
            nodes[i].initNode();
            nodes[i].allocationModified = handleNodeAllocationChanged;
        }

    }

    /// <summary>
    /// Remove or add a node to the player when a node allocation change
    /// </summary>
    /// <param name="iAllocated"></param>
    /// <param name="node"></param>
    void handleNodeAllocationChanged(bool iAllocated, UIPassiveNode node)
    {
        if (!iAllocated)
        {
            removeNodeFromPlayer(node);
        }
        else
        {
            addNodeToPlayer(node);
        }
                
    }

    /// <summary>
    /// Draw default all the nodes links
    /// </summary>
    public void drawDefaultNodeLink()
    {
        for (int i =0; i < nodes.Count; i++)
        {
            nodes[i].moveNodeToSavedPosition();
            nodes[i].drawLinks();
        }
    }

    /// <summary>
    /// Mode node to saved position
    /// </summary>
    public void moveNodesToSavedPosition()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].moveNodeToSavedPosition();
        }
    }


    /// <summary>
    /// save the nodes positions
    /// </summary>
    public void savePositions()
    {
        for (int i = 0; i < nodes.Count; i++)
            nodes[i].saveCurrentPosition();
    }

    #region loading and saving
    public void savePassiveTreeToJson()
    {
        List<UIPassiveNodeModel> nodesGUID = new List<UIPassiveNodeModel>();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodesGUID.Add(new UIPassiveNodeModel(nodes[i], resourcesList));
        }

        JsonWrappingClass<UIPassiveNodeModel> wrappingClass = new JsonWrappingClass<UIPassiveNodeModel>(nodesGUID);
        string treeAsJson = JsonUtility.ToJson(wrappingClass);
        File.WriteAllText(AssetDatabase.GetAssetPath(_passiveTreeSaveFile), treeAsJson);
    }

    public void loadPassiveTreeFromJson()
    {
        // clear skill tree
        _baseContainer.transform.clearChild();
        _nodeContainer.transform.clearChild();

        // load json file datas
        string passiveTreeAsJson = File.ReadAllText(AssetDatabase.GetAssetPath(_passiveTreeSaveFile));
        List<UIPassiveNodeModel> nodesModel = JsonUtility.FromJson<JsonWrappingClass<UIPassiveNodeModel>>(passiveTreeAsJson).elements;

        nodes = new List<UIPassiveNode>();

        // convert model to node
        for(int i = 0; i < nodesModel.Count; i++)
        {
            GameObject nodeInstance = Instantiate(nodesModel[i].isBase ? basePrefab : nodePrefab);
            UIPassiveNode node = nodesModel[i].modelToUIPassiveNode(nodeInstance.GetComponent<UIPassiveNode>(), resourcesList);
            node.transform.SetParent(node.isBase ? _baseContainer.transform : _nodeContainer.transform);

            // Remove later
            node.isAllocated = node.isBase;
            nodes.Add(node);
        }

        // assign node connected node
        foreach (UIPassiveNode node in nodes)
        {
            UIPassiveNodeModel nodeModel = nodesModel.Find(x => x.GUID == node.GUID);
            foreach (string connectedNodeGUID in nodeModel.connectedNodesGUID)
            {
                UIPassiveNode connectedNode = nodes.Find(x => x.GUID == connectedNodeGUID);
                node.connectedNodes.Add(connectedNode);
            }
        }
    }
    #endregion

    #region interaction
    protected override void leftClickOnUI(){}

    protected override void rightClickOnUI(){ }

    protected override void dragging(PointerEventData eventData)
    {
        transform.position = (Vector3) eventData.position - dragingOffset;
    }

    protected override void dragginEnd(PointerEventData eventData){}

    void zoom()
    {
        print(Input.mouseScrollDelta.y);
        transform.localScale = transform.localScale * ((Input.mouseScrollDelta.y * 0.1f) + 1);
    }
    #endregion

    /// <summary>
    /// Display or hide the skill based on his actual status (hidden or visible)
    /// </summary>
    public void displayTree()
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
    }
}
