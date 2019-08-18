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

    // Enemy Field
    string enemyName;
    float baseLife;
    float attackSpeedBonus;
    GameObject prefab;
    float movementSpeedBonus;


    public void showWindows(EnemyDatabase database, Enemy enemy = null, bool edit = false, int index = -1)
    {
        this.Show();
        this.database = database;
        this.edit = edit;
        this.databaseIndex = index;
        updateEnemy(enemy);
    }

    void OnGUI()
    { 
        EditorGUILayout.BeginVertical();
        enemyName = EditorGUILayout.TextField("Name : ", enemyName);
        baseLife = EditorGUILayout.FloatField("baseLife : ", baseLife);
        attackSpeedBonus = EditorGUILayout.FloatField("Attack Speed Bonus :", attackSpeedBonus);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefabs : ", prefab, typeof(GameObject), false);
        movementSpeedBonus = EditorGUILayout.FloatField("Movement Speed Bonus : ", movementSpeedBonus);

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
                updateEnemy();
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
                updateEnemy();
                database.addElement(enemy);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                this.Close();
            }
            if (GUILayout.Button("Add & continue"))
            {
                updateEnemy();
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

    void updateEnemy(Enemy enemyToEdit = null)
    {
        if(enemyToEdit == null)
        {
            enemy = new Enemy(enemyName, baseLife, baseLife, 0, attackSpeedBonus, 10, 0, new List<Skill>(), movementSpeedBonus, prefab, 0);
            enemy.interactibleType = InteractableObjectType.Enemy;
        }
        else
        {
            enemyName = enemyToEdit.name;
            baseLife = enemyToEdit.baseLife;
            attackSpeedBonus = enemyToEdit.bonusAttackSpeed;
            movementSpeedBonus = enemyToEdit.movementSpeedBonus;
            prefab = enemyToEdit.prefab;
        }
    }
}
