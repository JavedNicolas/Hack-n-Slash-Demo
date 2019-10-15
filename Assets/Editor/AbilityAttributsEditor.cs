using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityAttributs), true)]
public class AbilityAttributsEditor : Editor
{
    AbilityAttributs attributs;
    List<int> effectsIndex = new List<int>();
    List<bool> displayEffect = new List<bool>();

    private void OnEnable()
    {
        attributs = (AbilityAttributs)target;

        // init effec list sizes
        displayEffect.updateSize(attributs.effectAndValues.Count);
        effectsIndex.updateSize(attributs.effectAndValues.Count);

        for (int i = 0; i < attributs.effectAndValues.Count; i++)
        {
            effectsIndex[i] = attributs.effectAndValues[i].effectIndex;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        setEffectValueByLevelSize();

        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Ability Effects", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
        if (GUILayout.Button("Add Effect"))
        {
            addEffect();
        }
            
        EditorGUILayout.EndHorizontal();

        for(int i = 0; i < attributs.effectAndValues.Count; i++)
        {
            EditorGUILayout.Space();

            // effect title
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Effect n° " + i + " : ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
            if (GUILayout.Button(displayEffect[i] ? "Hide" : "Display"))
                displayEffect[i] = !displayEffect[i];
            if (GUILayout.Button("Remove") && EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete this ?", "Yes", "No"))
            {
                removeEffect(i);
            }
                
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel += 2;
            // hide or display effect
            if (displayEffect[i])
            {
                effectsIndex[i] = EditorGUILayout.Popup("Effect : ", effectsIndex[i], EffectList.getEffectsNames());

                attributs.effectAndValues[i].element = (Element)EditorGUILayout.EnumPopup("Effect Element :", attributs.effectAndValues[i].element);

                attributs.effectAndValues[i].effectType = (EffectUseBy)EditorGUILayout.EnumPopup("Effect used by :", attributs.effectAndValues[i].effectType);

                // value by level
                EditorGUILayout.LabelField("Value by Level :", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });

                for (int j = 0; j < attributs.effectAndValues[i].valuesByLevel.Count; j++)
                {
                    attributs.effectAndValues[i].valuesByLevel[j] = EditorGUILayout.FloatField("Value for level " + (i + 1) + " : ", attributs.effectAndValues[i].valuesByLevel[j]);
                }

                // stat types
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stat Types", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
                if (GUILayout.Button("Add"))
                    attributs.effectAndValues[i].statTypes.addEmptyElement();
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < attributs.effectAndValues[i].statTypes.Count; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    attributs.effectAndValues[i].statTypes[j] = (StatType)EditorGUILayout.EnumPopup(attributs.effectAndValues[i].statTypes[j]);
                    if (GUILayout.Button("Remove"))
                    {
                        if (EditorUtility.DisplayDialog("Are you sure ?", "Do you want to delete this ?", "Yes", "No"))
                        {
                            attributs.effectAndValues[i].statTypes.RemoveAt(j);
                        }
                    }
                        
                    EditorGUILayout.EndHorizontal();
                }

            }
            EditorGUI.indentLevel -= 2;
        }
    }

    void setEffectValueByLevelSize()
    {
        for(int i = 0; i < attributs.effectAndValues.Count; i++)
        {
            AbilityEffectAndValue effectAndValue = attributs.effectAndValues[i];
            effectAndValue.valuesByLevel.updateSize(attributs.maxLevel);
        }
    }

    void addEffect()
    {
        attributs.effectAndValues.addEmptyElement();
        effectsIndex.addEmptyElement();
        displayEffect.Add(true);
    }

    void removeEffect(int index)
    {
        attributs.effectAndValues.RemoveAt(index);
        effectsIndex.RemoveAt(index);
        displayEffect.RemoveAt(index);
    }
}
