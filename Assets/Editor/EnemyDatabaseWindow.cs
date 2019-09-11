using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EnemyDatabaseWindow : DatabaseWindows<Enemy>
{
    // database
    protected override string databasePath { get => "Databases/EnemyDatabase"; }
    ItemDatabase itemDatabase;

    Vector2 formScrollPos;

    // Enemy Field
    string enemyName;
    float baseLife;
    float attackSpeed;
    float castSpeed;
    GameObject prefab;
    float movementSpeed;
    InteractableObjectType interactableType;

    bool displayLoot = false;

    // loot
    int numberOfLoot;
    List<Loot> possibleLoots = new List<Loot>();
    List<int> lootID = new List<int>();

    //xp
    float xp;

    private void OnEnable()
    {
        itemDatabase = Resources.Load<ItemDatabase>(DatabaseConstant.itemDatabasePath);
    }

    [MenuItem("Database/Enemy")]
    public static void databadeMenuFunction()
    {
        windows = new EnemyDatabaseWindow();
        windows.showWindows();
    }

    #region Right panel (The form)
    protected override void displayForm()
    {
        formScrollPos = EditorGUILayout.BeginScrollView(formScrollPos);
        EditorGUILayout.BeginVertical();
        GUI.enabled = false;
        databaseID = EditorGUILayout.IntField("Database ID : ", databaseID);
        GUI.enabled = true;
        databaseID = database.getFreeId();
        enemyName = EditorGUILayout.TextField("Name : ", enemyName);
        xp = EditorGUILayout.FloatField("Base Experience %: ", xp);
        baseLife = EditorGUILayout.FloatField("Base Life : ", baseLife);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed :", attackSpeed);
        castSpeed = EditorGUILayout.FloatField("Cast Speed :", castSpeed);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefabs : ", prefab, typeof(GameObject), false);
        movementSpeed = EditorGUILayout.FloatField("Movement Speed : ", movementSpeed);
        interactableType = InteractableObjectType.Enemy;
        numberOfLoot = EditorGUILayout.IntField("Number Of Loot :", numberOfLoot);
        updateLootListsSize(numberOfLoot);

        // display of hide loot button
        if (numberOfLoot != 0)
        {
            string displayButtonText = displayLoot ? "Hide" : "Show";
            if (GUILayout.Button(displayButtonText + " loots"))
            {
                displayLoot = !displayLoot;
            }
        }

        if (numberOfLoot != 0 && displayLoot)
            for (int i = 0; i < numberOfLoot; i++)
                displayLootForm(i);


        displayFormButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Display the form to add loot
    /// </summary>
    /// <param name="index"></param>
    public void displayLootForm(int index)
    {
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
        EditorGUILayout.LabelField("Loot n° " + index + " : ", centerTitle);

        EditorGUILayout.BeginVertical("Box");

        EditorGUILayout.LabelField("Item :", centerTitle);
        possibleLoots[index].isRandom = EditorGUILayout.Toggle("Is a random Item :", possibleLoots[index].isRandom);
        if (!possibleLoots[index].isRandom)
        {
            if (possibleLoots[index].item != null)
                lootID[index] = possibleLoots[index].item.databaseID;
                lootID[index] = EditorGUILayout.IntField("Item Database Id : ", lootID[index]);
                if (lootID[index] != -1)
                {
                    Item item = itemDatabase.getElementWithDBID(lootID[index]);
                    if(item != null)
                    {
                        EditorGUILayout.LabelField("Item name : ", item.name, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold }); ;
                        possibleLoots[index].item = item;
                    }else
                        EditorGUILayout.LabelField("Can't find this item...");
                }
        }

        EditorGUILayout.LabelField("Chance to drop :", centerTitle);
        possibleLoots[index].changeToDrop = EditorGUILayout.FloatField(possibleLoots[index].changeToDrop);
        if (!possibleLoots[index].isRandom)
        {
            EditorGUILayout.LabelField("Quantity :", centerTitle);
            possibleLoots[index].quantity = EditorGUILayout.IntField(possibleLoots[index].quantity);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    void updateLootListsSize(int size)
    {
        while (possibleLoots.Count != size)
        {
            if (possibleLoots.Count < size)
            {
                possibleLoots.Add(new Loot(null, 0, 0));
            }
            else if (possibleLoots.Count > size)
            {
                possibleLoots.RemoveAt(possibleLoots.Count - 1);
            }
        }

        while (lootID.Count != size)
        {
            if (lootID.Count < size)
            {
                lootID.Add(-1);
            }
            else if (lootID.Count > size)
            {
                lootID.RemoveAt(lootID.Count - 1);
            }
        }

    }
    #endregion


    protected override void setFieldWithElementValues()
    { 
        if(element != null)
        {
            databaseID = element.databaseID;
            enemyName = element.name;
            xp = element.experience;
            baseLife = element.stats.maxLife;
            castSpeed = element.stats.castSpeed;
            attackSpeed = element.stats.attackSpeed;
            movementSpeed = element.stats.movementSpeed;
            prefab = element.prefab;
            numberOfLoot = element.possibleLoot.Count;
            possibleLoots = element.possibleLoot;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Enemy(enemyName, baseLife, attackSpeed, castSpeed, 10, movementSpeed, new List<Ability>(), prefab, possibleLoots, xp);
        element.interactibleType = interactableType;
        element.databaseID = databaseID;
    }

    protected override void clearForm()
    {
        base.clearForm();
        element = new Enemy();
        databaseID = database.getFreeId();
        enemyName = "";
        xp = 0;
        baseLife = 0;
        attackSpeed = 0;
        castSpeed = 0;
        movementSpeed = 0;
        prefab = null;
        numberOfLoot = 0;
        possibleLoots = new List<Loot>();
    }
}
