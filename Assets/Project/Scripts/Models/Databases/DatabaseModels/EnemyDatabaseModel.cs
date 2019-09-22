using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

[System.Serializable]
public class EnemyDatabaseModel : DatabaseElement
{
    [SerializeField] public List<Loot> possibleLoot = new List<Loot>();
    [SerializeField] public List<AbilityUsageFrequence> abilityUsageFrequency = new List<AbilityUsageFrequence>();
    [SerializeField] public BeingStats stats = new BeingStats();
    [SerializeField] public List<int> abilityIDs = new List<int>();
    [SerializeField] public string prefabGUID;
    [SerializeField] public float experience;
    [SerializeField] public InteractableObjectType interactableType;

    public EnemyDatabaseModel(int databaseID, string name, InteractableObjectType interactableType, BeingStats stats, List<int> abilityIDs, List<Loot> possibleLoot, List<AbilityUsageFrequence> abilityUsageFrequency, string prefabGUI, float experience)
    {
        this.databaseID = databaseID;
        this.name = name;
        this.possibleLoot = possibleLoot;
        this.abilityUsageFrequency = abilityUsageFrequency;
        this.stats = stats;
        this.abilityIDs = abilityIDs;
        this.prefabGUID = prefabGUI;
        this.experience = experience;
        this.interactableType = interactableType;
    }

    public Enemy databaseModelToEnemy(DatabaseResourcesList resourcesList)
    {
        GameObject prefab = (GameObject)resourcesList.getObject<GameObject>(prefabGUID);
        Enemy enemy = new Enemy(name, stats.maxLife, stats.attackSpeed, stats.castSpeed, stats.attackRange,
            stats.movementSpeed, abilityIDs, abilityUsageFrequency, prefab, possibleLoot, experience);
        enemy.databaseID = databaseID;
        enemy.interactibleType = interactableType;
        return enemy;
    }

}


