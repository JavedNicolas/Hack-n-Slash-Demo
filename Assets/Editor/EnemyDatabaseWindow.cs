using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EnemyDatabaseWindow : DatabaseWindows<Enemy>
{
    // database
    protected override string databasePath { get => "Databases/EnemyDatabase"; }

    // Enemy Field
    string enemyName;
    float baseLife;
    float attackSpeedBonus;
    GameObject prefab;
    float movementSpeedBonus;
    InteractableObjectType interactableType;

    [MenuItem("Database/Enemy")]
    public static void databadeMenuFunction()
    {
        windows = new EnemyDatabaseWindow();
        windows.showWindows();
    }

    #region Right panel (The form)
    protected override void displayForm()
    {
        EditorGUILayout.BeginVertical();
        GUI.enabled = false;
        databaseID = EditorGUILayout.IntField("Database ID : ", databaseID);
        GUI.enabled = true;
        databaseID = database.getFreeId();
        enemyName = EditorGUILayout.TextField("Name : ", enemyName);
        baseLife = EditorGUILayout.FloatField("baseLife : ", baseLife);
        attackSpeedBonus = EditorGUILayout.FloatField("Attack Speed Bonus :", attackSpeedBonus);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefabs : ", prefab, typeof(GameObject), false);
        movementSpeedBonus = EditorGUILayout.FloatField("Movement Speed Bonus : ", movementSpeedBonus);
        interactableType = (InteractableObjectType)EditorGUILayout.EnumPopup("Interactible type : ", interactableType);
        displayFormButtons();
        EditorGUILayout.EndVertical();
    }
    #endregion


    protected override void setFieldWithElementValues()
    { 
        if(element != null)
        {
            databaseID = element.databaseID;
            enemyName = element.name;
            baseLife = element.baseLife;
            attackSpeedBonus = element.bonusAttackSpeed;
            movementSpeedBonus = element.movementSpeedBonus;
            prefab = element.prefab;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Enemy(enemyName, baseLife, baseLife, 0, attackSpeedBonus, 10, 0, new List<Ability>(), movementSpeedBonus, prefab, 0);
        element.interactibleType = interactableType;
        element.databaseID = databaseID;
    }

    protected override void clearForm()
    {
        base.clearForm();
        element = new Enemy();
        databaseID = database.getFreeId();
        enemyName = "";
        baseLife = 0;
        attackSpeedBonus = 0;
        movementSpeedBonus = 0;
        prefab = null;
    }
}
