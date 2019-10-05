using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPassiveSkillTree))]
public class UIPassiveSkillTreeEditor : Editor
{
    UIPassiveSkillTree skillTree;

    private void OnEnable()
    {
        skillTree = (UIPassiveSkillTree)target;
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
        skillTree.drawDefaultNodeLink();
    }

    void saveSkillTree()
    {
        skillTree.getChildNodes();
        skillTree.savePositions();
        skillTree.initNodes();
        skillTree.savePassiveTreeToJson();
    }
}