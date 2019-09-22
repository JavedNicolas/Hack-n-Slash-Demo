using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EnemyDatabaseWindow : DatabaseWindows<EnemyDatabaseModel>
{
    // database
    protected override string databasePath { get => "Databases/EnemyDatabase"; }

    Vector2 formScrollPos;

    // Enemy Field
    string enemyName;
    float baseLife;
    float attackSpeed;
    float castSpeed;
    GameObject prefab;
    float movementSpeed;
    InteractableObjectType interactableType;
    int numberOfLoot;
    int numberOfAbility;
    List<int> lootID = new List<int>();
    List<int> abilityIDs = new List<int>();
    bool displayLootAndAbility = false;
    float xp;

    // loot
    List<Loot> possibleLoots = new List<Loot>();
    List<Ability> abilities = new List<Ability>();
    List<AbilityUsageFrequence> abilityFrequency = new List<AbilityUsageFrequence>();

    // tmp value only used in the editor
    // display ability and loot
    ItemDatabase itemDatabase;
    AbilityDatabase abilityDatabase;

    private void OnEnable()
    {
        itemDatabase = Resources.Load<ItemDatabase>(DatabaseConstant.itemDatabasePath);
        abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
    }

    #region Right panel (The form)
    protected override void displayForm()
    {
        formScrollPos = EditorGUILayout.BeginScrollView(formScrollPos);
        EditorGUILayout.BeginVertical();
        GUI.enabled = false;
        databaseID = EditorGUILayout.IntField("Database ID : ", databaseID);
        GUI.enabled = true;
        enemyName = EditorGUILayout.TextField("Name : ", enemyName);
        xp = EditorGUILayout.FloatField("Base Experience %: ", xp);
        baseLife = EditorGUILayout.FloatField("Base Life : ", baseLife);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed :", attackSpeed);
        castSpeed = EditorGUILayout.FloatField("Cast Speed :", castSpeed);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefabs : ", prefab, typeof(GameObject), false);
        movementSpeed = EditorGUILayout.FloatField("Movement Speed : ", movementSpeed);
        interactableType = InteractableObjectType.Enemy;
        numberOfLoot = EditorGUILayout.IntField("Number Of Loot :", numberOfLoot);
        numberOfAbility = EditorGUILayout.IntField("Number Of Ability :", numberOfAbility);
        updateLootListsSize(numberOfLoot);
        updateAbilityListsSize(numberOfAbility);

        // display of hide loot button
        if (numberOfLoot != 0 || numberOfAbility != 0)
        {
            string displayButtonText = displayLootAndAbility ? "Hide" : "Show";
            if (GUILayout.Button(displayButtonText + " loots and abilies"))
            {
                displayLootAndAbility = !displayLootAndAbility;
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (numberOfLoot != 0 && displayLootAndAbility)
            displayLootsForm();

        if (numberOfAbility != 0 && displayLootAndAbility)
            displaySkillsForm();
        EditorGUILayout.EndHorizontal();


        displayFormButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Display the form to add loot
    /// </summary>
    public void displayLootsForm()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        for (int index = 0; index < numberOfLoot; index++)
        {
            EditorGUILayout.LabelField("Loot n° " + index + " : ", centerTitle);

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Item :", centerTitle);
            possibleLoots[index].isRandom = EditorGUILayout.Toggle("Is a random Item :", possibleLoots[index].isRandom);
            if (!possibleLoots[index].isRandom)
            {
                lootID[index] = EditorGUILayout.IntField("Item Database Id : ", lootID[index]);
                if (lootID[index] != -1)
                {
                    ItemDatabaseModel itemDatabaseModel = itemDatabase.getElementWithDBID(lootID[index]);
                    if (itemDatabaseModel != null)
                    {
                        EditorGUILayout.LabelField("Item name : ", itemDatabaseModel.name, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold }); ;
                    }
                    else
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
        }
        EditorGUILayout.EndVertical();
    }
            

    public void displaySkillsForm()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        for (int index = 0; index < numberOfAbility; index++)
        {
            EditorGUILayout.LabelField("Skill n° " + index + " : ", centerTitle);

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.LabelField("Ability :", centerTitle);

            abilityIDs[index] = EditorGUILayout.IntField("Skill Database Id : ", abilityIDs[index]);

            if (abilityIDs[index] != -1)
            {
                AbilityDatabaseModel ability = abilityDatabase.getElementWithDBID(abilityIDs[index]);
                if (ability != null)
                {
                    EditorGUILayout.LabelField("Ability name : ", ability.getName(), new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold }); ;
                }
                else
                    EditorGUILayout.LabelField("Can't find this Ability...");
            }

            EditorGUILayout.LabelField("Usage Frenquency :", centerTitle);
            abilityFrequency[index] = (AbilityUsageFrequence)EditorGUILayout.EnumPopup(abilityFrequency[index]);

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// Update the possible loot array size
    /// </summary>
    /// <param name="size"></param>
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

    /// <summary>
    /// Update the ability array size
    /// </summary>
    /// <param name="size"></param>
    void updateAbilityListsSize(int size)
    {
        while (abilities.Count != size)
        {
            if (abilities.Count < size)
            {
                abilities.Add(null);
            }
            else if (abilities.Count > size)
            {
                abilities.RemoveAt(abilities.Count - 1);
 
            }
        }

        while (abilityFrequency.Count != size)
        {
            if (abilityFrequency.Count < size)
            {
                abilityFrequency.Add(AbilityUsageFrequence.Normal);
            }
            else if (abilityFrequency.Count > size)
            {
                abilityFrequency.RemoveAt(abilityFrequency.Count - 1);
            }
        }

        while (abilityIDs.Count != size)
        {
            if (abilityIDs.Count < size)
            {
                abilityIDs.Add(-1);
            }
            else if (abilityIDs.Count > size)
            {
                abilityIDs.RemoveAt(abilityIDs.Count - 1);
            }
        }
    }
    #endregion

    protected override void setFieldWithElementValues()
    { 
        if(element != null)
        {
            Enemy enemy = element.databaseModelToEnemy(resourcesList);
            databaseID = enemy.databaseID;
            enemyName = enemy.name;
            xp = enemy.experience;
            baseLife = enemy.stats.maxLife;
            castSpeed = enemy.stats.castSpeed;
            attackSpeed = enemy.stats.attackSpeed;
            movementSpeed = enemy.stats.movementSpeed;
            prefab = enemy.prefab;
            numberOfLoot = enemy.possibleLoot.Count;
            possibleLoots = enemy.possibleLoot;
            numberOfAbility = enemy.abilityIDs.Count;
            abilityIDs = enemy.abilityIDs;
            abilities = new List<Ability>();
            abilityFrequency = enemy.abilityUsageFrequency;
        }
    }

    protected override void updateElementWithFormValues()
    {
        BeingStats stats = new BeingStats(baseLife, attackSpeed, castSpeed, attackSpeed, movementSpeed);
        element = new EnemyDatabaseModel(databaseID, enemyName, interactableType, stats, abilityIDs, possibleLoots, abilityFrequency, resourcesList.addObjects(prefab), xp); 
    }

    protected override void clearForm()
    {
        base.clearForm();
        databaseID = database.getFreeId();
        element = null;
        enemyName = "";
        xp = 0;
        baseLife = 0;
        attackSpeed = 0;
        castSpeed = 0;
        movementSpeed = 0;
        prefab = null;
        numberOfLoot = 0;
        possibleLoots = new List<Loot>();
        numberOfAbility = 0;
        abilityIDs = new List<int>();
        abilities = new List<Ability>();
        abilityFrequency = new List<AbilityUsageFrequence>();
    }
}
