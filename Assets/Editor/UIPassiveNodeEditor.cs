using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UIPassiveNode))]
public class UIPassiveNodeEditor : Editor
{
    UIPassiveNode uiPassiveNode;
    PassiveNode passiveNode;
    GUIStyle titleStyle;

    private void OnEnable()
    {
        uiPassiveNode = (UIPassiveNode)target;
    }

    public override void OnInspectorGUI()
    {
        passiveNode = uiPassiveNode.node == null ? new PassiveNode() : uiPassiveNode.node;

        titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("UI Node Attributs :", titleStyle);
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel += 2;
        base.DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUI.indentLevel-=2;
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Node Editor :", titleStyle);
        EditorGUILayout.EndVertical();

        EditorGUI.indentLevel += 2;
        // save node
        if (GUILayout.Button("Save Node Effect"))
            uiPassiveNode.setNode(passiveNode);

        passiveNode.name = EditorGUILayout.TextField("Name : ", passiveNode.name);
        passiveNode.icon = (Sprite)EditorGUILayout.ObjectField("Icon : ", passiveNode.icon, typeof(Sprite), false);
        passiveNode.maxLevel = EditorGUILayout.IntField("Max level : ", passiveNode.maxLevel);

        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Stats :", titleStyle);
        if (GUILayout.Button("+"))
            addStat();
        EditorGUILayout.EndHorizontal();

        displayStats();
    }

    void addStat()
    {
        passiveNode.nodeStats.Add(new Stat());
    }

    void displayStats()
    {

        for(int i = 0; i< passiveNode.nodeStats.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stat n°" + i +" :", titleStyle);
            if (GUILayout.Button("x"))
            {
                passiveNode.nodeStats.RemoveAt(i);
                displayStats();
                break;
            } 
            EditorGUILayout.EndHorizontal();

            passiveNode.nodeStats[i].statType = (StatType)EditorGUILayout.EnumPopup("Stat Type : ", passiveNode.nodeStats[i].statType);
            passiveNode.nodeStats[i].bonusType = (StatBonusType)EditorGUILayout.EnumPopup("Bonus Type : ", passiveNode.nodeStats[i].bonusType);
            passiveNode.nodeStats[i].value = EditorGUILayout.FloatField("Value : ", passiveNode.nodeStats[i].value);
            passiveNode.nodeStats[i].isInfluencedBy = (StatInfluencedBy)EditorGUILayout.EnumPopup("Is Influence by an other Stat : ", passiveNode.nodeStats[i].isInfluencedBy);
            if(passiveNode.nodeStats[i].isInfluencedBy != StatInfluencedBy.Nothing)
                passiveNode.nodeStats[i].influencedEvery = EditorGUILayout.FloatField("Influence Every : ", passiveNode.nodeStats[i].influencedEvery);
            passiveNode.nodeStats[i].isSpecific = EditorGUILayout.Toggle("Is Specific Something :", passiveNode.nodeStats[i].isSpecific);
            if (passiveNode.nodeStats[i].isSpecific)
                passiveNode.nodeStats[i].isSpecificTo = EditorGUILayout.TextField("Is Specific To (Name): ", passiveNode.nodeStats[i].isSpecificTo);
            passiveNode.nodeStats[i].sourceName = passiveNode.name;

            EditorGUILayout.Space();
        }
    }
}