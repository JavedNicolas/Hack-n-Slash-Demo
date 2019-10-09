using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPassiveTree))]
public class UIPassiveSkillTreeEditor : Editor
{
    UIPassiveTree skillTree;

    private void OnEnable()
    {
        skillTree = (UIPassiveTree)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save Skill Tree"))
            saveSkillTree();

        if (GUILayout.Button("Load Passive Tree"))
            updateTree();
        base.OnInspectorGUI();
    }

    void updateTree()
    {
        skillTree.loadPassiveTreeFromJson();
        skillTree.initNodes();
        skillTree.moveNodesToSavedPosition();
    }

    void saveSkillTree()
    {
        skillTree.getChildNodes();
        skillTree.savePositions();
        skillTree.initNodes();
        skillTree.savePassiveTreeToJson();
    }
}