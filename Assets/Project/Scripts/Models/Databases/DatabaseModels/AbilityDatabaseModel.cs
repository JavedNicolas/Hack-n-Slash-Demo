using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class AbilityDatabaseModel : DatabaseElement
{
    [SerializeField] public string attributsGuid;

    public AbilityDatabaseModel(Ability ability, string attributsGUID)
    {
        this.databaseID = ability.databaseID;
        this.name = ability.getName();
        this.attributsGuid = attributsGUID;
    }

    public AbilityDatabaseModel(int databaseID, string name, string attributsGuid)
    {
        this.databaseID = databaseID;
        this.name = name;
        this.attributsGuid = attributsGuid;
    }

    public Ability abilityModelToAbility(DatabaseResourcesList resourcesList, AbilityDatabase abilityDatabase)
    {
        AbilityAttributs attributs = (AbilityAttributs)resourcesList.getObject<ScriptableObject>(attributsGuid);
        Ability ability = abilityDatabase.abilities.Find(x => x.getName() == name);
        ability.abilityAttributs = attributs;
        ability.databaseID = databaseID;

        return ability;
    }
}
