using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor
{
    [SerializeField] EnemyDatabase enemyDatabase;

    private void OnEnable()
    {
        enemyDatabase = (EnemyDatabase)target;
        EditorUtility.SetDirty(enemyDatabase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    override public void OnInspectorGUI()
    {
        if (GUILayout.Button("Add an enemy")){
            EnemyDatabaseWindow windows = new EnemyDatabaseWindow();
            windows.showWindows(enemyDatabase, new Enemy());
        }

        for (int i =0; i < enemyDatabase.getDatabaseSize(); i++)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(enemyDatabase.getElementAt(i).name);
            displayEnemyButton(i);

            EditorGUILayout.EndVertical();
        }
    }

    void displayEnemyButton(int i)
    {

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit"))
        {
            EnemyDatabaseWindow windows = new EnemyDatabaseWindow();
            windows.showWindows(enemyDatabase, enemyDatabase.getElementAt(i), true, i);
        }
        if (GUILayout.Button("Remove"))
        {
            enemyDatabase.removeElement(enemyDatabase.getElementAt(i));
        }
        EditorGUILayout.EndHorizontal();
    }

}
