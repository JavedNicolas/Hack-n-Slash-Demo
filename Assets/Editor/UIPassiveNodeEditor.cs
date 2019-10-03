using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPassiveNode))]
public class UIPassiveNodeEditor : Editor
{
    UIPassiveNode passiveNode;

    private void OnEnable()
    {
        passiveNode = (UIPassiveNode)target;
        passiveNode.lineRenderer = passiveNode.GetComponent<LineRenderer>();
    }

    public override void OnInspectorGUI()
    {
        passiveNode.setPosition();
        base.OnInspectorGUI();
    }
}