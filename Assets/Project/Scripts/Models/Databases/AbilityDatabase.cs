using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "Database/Ability", fileName = DatabaseConstant.abilityDatabaseFileName)]
public class AbilityDatabase : Database<AbilityDatabaseModel>
{
    [SerializeField] public List<Ability> abilities = AbilityList.list;

    public void initElements(DatabaseResourcesList resourcesList)
    {
        loadDB();
        for (int i =0; i < abilities.Count; i++)
        {
            AbilityDatabaseModel model = elements.Find(x => x.getName() == abilities[i].getName());
            if (model == null){
                model = new AbilityDatabaseModel(getFreeId(), abilities[i].getName(), "");
                elements.Add(model);
            }
        }
    }

    public Ability getAbilityFromDatabaseID(int databaseID, DatabaseResourcesList resourcesList)
    {
        AbilityDatabaseModel databaseModel = elements.Find(x => x.databaseID == databaseID);
        if (databaseModel == null)
            return null;

        Ability ability = databaseModel.abilityModelToAbility(resourcesList, this);
        return ability;

    }
}