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

        if (attributs.effectAndValues.effects.Count > attributs.effectAndValues.effectValues.Count)
            while (attributs.effectAndValues.effectValues.Count < attributs.effectAndValues.effects.Count)
                attributs.effectAndValues.effectValues.Add(0);
        else if (attributs.effectAndValues.effects.Count < attributs.effectAndValues.effectValues.Count)
            while (attributs.effectAndValues.effectValues.Count > attributs.effectAndValues.effects.Count)
                attributs.effectAndValues.effectValues.RemoveAt(attributs.effectAndValues.effects.Count - 1);
    }

}
