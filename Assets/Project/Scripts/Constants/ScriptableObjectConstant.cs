using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptableObjectConstant
{
    public const string abilityAttributsMenuName = "Ability/Ability Attributs/";
    
    // effects
    public const string effectMenuName = "Effect/";
    public const string effectFolderPath = "Resources/ScriptableObjects/Effects/";

    // ressources lists
    public const string resourceListPath = "ScriptableObjects/DatabaseRessourcesList";

    // localization
    public const string localizedTextPath = "ScriptableObjects/LocalizedText";
    public const string FullLocalizedJsonFolder = "Assets/Project/Resources/JsonFiles/LocalizatedTextJSONs/"; 
    public const string ResourceLocalizedJsonFolder = "JsonFiles/LocalizatedTextJSONs/";
}

public static class DatabaseConstant
{
    public const string databaseFolder = "Databases/";
    public const string abilityDatabaseFileName = "AbilityDatabase";
    public const string enemyDatabaseFileName = "EnemyDatabase";
    public const string itemDatabaseFileName = "ItemDatabase";
    public const string itemDatabasePath = databaseFolder + itemDatabaseFileName;
    public const string abilityDatabasePath = databaseFolder + abilityDatabaseFileName;
}
