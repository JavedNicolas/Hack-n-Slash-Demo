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

    public EnemyDatabaseModel(Enemy enemy, DatabaseResourcesList resourcesList)
    {
        this.databaseID = enemy.databaseID;
        this.name = enemy.name;
        this.abilityUsageFrequency = enemy.abilityUsageFrequency;
        this.stats = enemy.stats;
        this.abilityIDs = enemy.abilityIDs;
        this.prefabGUID = resourcesList.getGUIDFor(enemy.model);
        this.experience = enemy.experience;
        this.interactbileType = enemy.interactibleType;

        List<LootDatabaseModel> lootDatabaseModels = new List<LootDatabaseModel>();
        foreach (Loot loot in enemy.possibleLoot)
            lootDatabaseModels.Add(new LootDatabaseModel(loot));

        this.possibleLootsDatabaseModel = lootDatabaseModels;
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


