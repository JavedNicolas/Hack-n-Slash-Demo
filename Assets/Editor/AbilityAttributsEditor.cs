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
    }

}
