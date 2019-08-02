using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyDatabaseWindow : EditorWindow
{
    EnemyDatabase database;
    bool edit = false;
    Enemy enemy;
    int databaseIndex;

    public void showWindows(EnemyDatabase database, Enemy enemy, bool edit = false, int index = -1)
    {
        this.Show();
        this.database = database;
        this.edit = edit;
        this.enemy = enemy;
        this.databaseIndex = index;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        enemy.name = EditorGUILayout.TextField("Name : ", enemy.name);
        enemy.baseLife = EditorGUILayout.FloatField("baseLife : ", enemy.baseLife);
        enemy.aspd = EditorGUILayout.FloatField("Attack per Second :", enemy.aspd);
        enemy.prefab = (GameObject)EditorGUILayout.ObjectField("Prefabs : ", enemy.prefab, typeof(GameObject), false);
        enemy.movementSpeedPercentage = EditorGUILayout.FloatField("Movement Speed Percentage : ", enemy.movementSpeedPercentage);

        EditorGUILayout.EndVertical();
        displayButtons();
    }

    void displayButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (edit)
        {
            if (GUILayout.Button("Update"))
            {
                database.updateElement(enemy, databaseIndex);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                this.Close();
            }
        }
        else
        {
            if (GUILayout.Button("Add"))
            {
                database.addElement(enemy);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                this.Close();
            }
            if (GUILayout.Button("Add & continue"))
            {
                database.addElement(enemy);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                enemy = new Enemy();
            }
        }
       
        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}
