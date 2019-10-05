using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;

public class UIPassiveSkillTree : MonoBehaviour
{
    [SerializeField] Object _passiveTreeSaveFile;

    [Header("Skill Tree Elements")]
    [SerializeField] GameObject _baseContainer;
    [SerializeField] GameObject _nodeContainer;

    [Header("Node links attributs")]
    [SerializeField] GameObject basePrefab;
    [SerializeField] GameObject nodePrefab;

    [SerializeField] List<UIPassiveNode> nodes = new List<UIPassiveNode>();

    private void Start()
    {
        loadPassiveTreeFromJson();
        initNodes();
        moveNodesToSavedPosition();
        drawDefaultNodeLink();
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
        for (int i = 0; i < nodes.Count; i++)
            nodes[i].initNode();
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
     
    public void savePassiveTreeToJson()
    {
        List<UIPassiveNodeModel> nodesGUID = new List<UIPassiveNodeModel>();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodesGUID.Add(new UIPassiveNodeModel(nodes[i]));
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
            nodeInstance.name = i.ToString();
            UIPassiveNode node = nodesModel[i].modelToUIPassiveNode(nodeInstance.GetComponent<UIPassiveNode>());
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


}
