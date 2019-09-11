using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityAttributs), true)]
public class AbilityAttributsEditor : Editor
{
    AbilityAttributs attributs;

    private void OnEnable()
    {
        attributs = (AbilityAttributs)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        setEffectValueByLevelSize();
    }

    void setEffectValueByLevelSize()
    {
        for(int i = 0; i < attributs.effectAndValues.Count; i++)
        {
            AbilityEffectAndValue effectAndValue = attributs.effectAndValues[i];
            while(effectAndValue.valuesByLevel.Count != attributs.maxLevel)
            {
                if(effectAndValue.valuesByLevel.Count < attributs.maxLevel)
                    effectAndValue.valuesByLevel.Add(0);
                if (effectAndValue.valuesByLevel.Count > attributs.maxLevel)
                    effectAndValue.valuesByLevel.RemoveAt(effectAndValue.valuesByLevel.Count -1);
            }
        }
    }
}
