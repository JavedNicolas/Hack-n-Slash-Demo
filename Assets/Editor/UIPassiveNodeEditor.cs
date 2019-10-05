using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPassiveNode))]
public class UIPassiveNodeEditor : Editor
{
    UIPassiveNode passiveNode;

    private void OnEnable()
    {
        passiveNode = (UIPassiveNode)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}