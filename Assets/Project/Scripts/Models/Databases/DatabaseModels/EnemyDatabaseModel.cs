using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

[System.Serializable]
public class EnemyDatabaseModel : DatabaseElement
{
    [SerializeField] public List<LootDatabaseModel> possibleLootsDatabaseModel = new List<LootDatabaseModel>();
    [SerializeField] public List<AbilityUsageFrequence> abilityUsageFrequency = new List<AbilityUsageFrequence>();
    [SerializeField] public BeingStats stats = new BeingStats();
    [SerializeField] public List<int> abilityIDs = new List<int>();
    [SerializeField] public string prefabGUID;
    [SerializeField] public float experience;
    [SerializeField] public InteractableObjectType interactbileType;

    public EnemyDatabaseModel(Enemy enemy, string prefabGUI, List<LootDatabaseModel> lootDatabaseModel)
    {
        this.databaseID = enemy.databaseID;
        this.name = enemy.name;
        this.possibleLootsDatabaseModel = lootDatabaseModel;
        this.abilityUsageFrequency = enemy.abilityUsageFrequency;
        this.stats = enemy.stats;
        this.abilityIDs = enemy.abilityIDs;
        this.prefabGUID = prefabGUI;
        this.experience = enemy.experience;
        this.interactbileType = enemy.interactibleType;
    }

    public EnemyDatabaseModel(int databaseID, string name, InteractableObjectType interactibleType, BeingStats stats, List<int> abilityIDs, List<LootDatabaseModel> possibleLoot, List<AbilityUsageFrequence> abilityUsageFrequency, string prefabGUI, float experience)
    {
        this.databaseID = databaseID;
        this.name = name;
        this.possibleLootsDatabaseModel = possibleLoot;
        this.abilityUsageFrequency = abilityUsageFrequency;
        this.stats = stats;
        this.abilityIDs = abilityIDs;
        this.prefabGUID = prefabGUI;
        this.experience = experience;
        this.interactbileType = interactibleType;
    }

    public Enemy databaseModelToEnemy(DatabaseResourcesList resourcesList, ItemDatabase itemDatabase)
    {
        GameObject prefab = (GameObject)resourcesList.getObject<GameObject>(prefabGUID);

        List<Loot> possibleLoots = new List<Loot>();
        if(possibleLootsDatabaseModel != null)
            foreach(LootDatabaseModel lootDatabaseModel in possibleLootsDatabaseModel)
            {
                possibleLoots.Add(lootDatabaseModel.databaseModelToLoot(resourcesList, itemDatabase));
            }

        Enemy enemy = new Enemy(name, stats.maxLife, stats.attackSpeed, stats.castSpeed, stats.attackRange,
            stats.movementSpeed, abilityIDs, abilityUsageFrequency, prefab, possibleLoots, experience);
        enemy.databaseID = databaseID;
        enemy.interactibleType = interactbileType;
        return enemy;
    }
}


