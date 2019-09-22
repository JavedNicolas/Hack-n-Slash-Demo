using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableObjectConstant
{
    public const string databaseFolder = "Databases/";
    public const string abilityAttributsMenuName = "Ability/Ability Attributs/";
    public const string effectMenuName = "Effect/";
    public const string resourceListPath = "ScriptableObjects/RessourcesList";
}

public static class DatabaseConstant
{
    public const string abilityDatabaseFileName = "AbilityDatabase";
    public const string enemyDatabaseFileName = "EnemyDatabase";
    public const string itemDatabaseFileName = "ItemDatabase";
    public const string itemDatabasePath = ScriptableObjectConstant.databaseFolder + itemDatabaseFileName;
    public const string abilityDatabasePath = ScriptableObjectConstant.databaseFolder + abilityDatabaseFileName;
}
