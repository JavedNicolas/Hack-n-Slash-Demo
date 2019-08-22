using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor
{
    EnemyDatabase enemyDatabase;

    private void OnEnable()
    {
        enemyDatabase = (EnemyDatabase)target;
        EditorUtility.SetDirty(enemyDatabase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    override public void OnInspectorGUI()
    {

        if (GUILayout.Button("Show Database")){
            new EnemyDatabaseWindow().showWindows();
        }
    }

   

}
