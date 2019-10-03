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
        if(GUILayout.Button("Update Links"))
        {
            updateTree();
        }

        base.OnInspectorGUI();
    }

    void updateTree()
    {
        skillTree.updateNodes();
        skillTree.drawNodeLink();
    }
}