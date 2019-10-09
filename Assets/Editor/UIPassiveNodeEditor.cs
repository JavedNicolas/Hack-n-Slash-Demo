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
        passiveNode.maxLevel = EditorGUILayout.IntField("Max level : ", passiveNode.maxLevel > 3 ? 3 : passiveNode.maxLevel);
         
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Stats :", titleStyle);
        if (GUILayout.Button("+"))
            addStat();
        EditorGUILayout.EndHorizontal();

        displayStats();
    }

    void addStat()
    {
        passiveNode.stats.Add(new Stat());
    }

    void displayStats()
    {

        for(int i = 0; i< passiveNode.stats.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stat n°" + i +" :", titleStyle);
            if (GUILayout.Button("x"))
            {
                passiveNode.stats.RemoveAt(i);
                displayStats();
                break;
            } 
            EditorGUILayout.EndHorizontal();

            passiveNode.stats[i].statType = (StatType)EditorGUILayout.EnumPopup("Stat Type : ", passiveNode.stats[i].statType);
            passiveNode.stats[i].bonusType = (StatBonusType)EditorGUILayout.EnumPopup("Bonus Type : ", passiveNode.stats[i].bonusType);
            passiveNode.stats[i].value = EditorGUILayout.FloatField("Value : ", passiveNode.stats[i].value);
            passiveNode.stats[i].isInfluencedBy = (StatInfluencedBy)EditorGUILayout.EnumPopup("Is Influence by an other Stat : ", passiveNode.stats[i].isInfluencedBy);
            if(passiveNode.stats[i].isInfluencedBy != StatInfluencedBy.Nothing)
                passiveNode.stats[i].influencedEvery = EditorGUILayout.FloatField("Influence Every : ", passiveNode.stats[i].influencedEvery);
            passiveNode.stats[i].isSpecific = EditorGUILayout.Toggle("Is Specific Something :", passiveNode.stats[i].isSpecific);
            if (passiveNode.stats[i].isSpecific)
                passiveNode.stats[i].isSpecificTo = EditorGUILayout.TextField("Is Specific To (Name): ", passiveNode.stats[i].isSpecificTo);
            passiveNode.stats[i].sourceName = passiveNode.name;

            EditorGUILayout.Space();
        }
    }
}