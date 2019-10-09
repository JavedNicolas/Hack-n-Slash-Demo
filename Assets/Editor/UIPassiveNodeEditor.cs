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
        passiveNode.stats.Add(new PassiveNodeStat());
    }

    void displayStats()
    {
        // display all the stats
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

            passiveNode.stats[i].stat.statType = (StatType)EditorGUILayout.EnumPopup("Stat Type : ", passiveNode.stats[i].stat.statType);
            passiveNode.stats[i].stat.bonusType = (StatBonusType)EditorGUILayout.EnumPopup("Bonus Type : ", passiveNode.stats[i].stat.bonusType);

            EditorGUI.indentLevel += 2;
            // values by level
            displayLevelValues(i);
            EditorGUI.indentLevel -= 2;

            passiveNode.stats[i].stat.isInfluencedBy = (StatInfluencedBy)EditorGUILayout.EnumPopup("Is Influence by an other Stat : ", passiveNode.stats[i].stat.isInfluencedBy);
            if(passiveNode.stats[i].stat.isInfluencedBy != StatInfluencedBy.Nothing)
                passiveNode.stats[i].stat.influencedEvery = EditorGUILayout.FloatField("Influence Every : ", passiveNode.stats[i].stat.influencedEvery);
            passiveNode.stats[i].stat.isSpecific = EditorGUILayout.Toggle("Is Specific Something :", passiveNode.stats[i].stat.isSpecific);
            if (passiveNode.stats[i].stat.isSpecific)
                passiveNode.stats[i].stat.isSpecificTo = EditorGUILayout.TextField("Is Specific To (Name): ", passiveNode.stats[i].stat.isSpecificTo);
            passiveNode.stats[i].stat.sourceName = passiveNode.name;

            EditorGUILayout.Space();
        }
    }


    /// <summary>
    /// Display the field to implement the values by level
    /// </summary>
    /// <param name="index"></param>
    void displayLevelValues(int index)
    {
        
        EditorGUILayout.LabelField("Values by level : ", titleStyle);
        while (passiveNode.stats[index].levelValue.Count != passiveNode.maxLevel)
        {
            if (passiveNode.stats[index].levelValue.Count > passiveNode.maxLevel)
                passiveNode.stats[index].levelValue.RemoveAt(passiveNode.stats[index].levelValue.Count - 1);
            if (passiveNode.stats[index].levelValue.Count < passiveNode.maxLevel)
                passiveNode.stats[index].levelValue.Add(0);
        }

        for (int j = 0; j < passiveNode.maxLevel; j++)
        {
            passiveNode.stats[index].levelValue[j] = EditorGUILayout.FloatField("Value for Level " + index + " : ", passiveNode.stats[index].levelValue[j]);
        }
    }
}