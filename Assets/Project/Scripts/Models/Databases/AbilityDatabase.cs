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

    public void initElements(ResourcesList resourcesList)
    {
        loadDB();
        for (int i =0; i < abilities.Count; i++)
        {
            AbilityDatabaseModel model = elements.Find(x => x.getName() == abilities[i].getName());
            if (model == null){
                Ability ability = (Ability)Activator.CreateInstance(abilities[i].GetType());
                ability.databaseID = getFreeId();
                model = new AbilityDatabaseModel(ability, resourcesList);
                elements.Add(model);
            }
        }
    }

    public Ability getAbilityFromDatabaseID(int databaseID, ResourcesList resourcesList)
    {
        AbilityDatabaseModel databaseModel = elements.Find(x => x.databaseID == databaseID);
        if (databaseModel == null)
            return null;

        Ability ability = databaseModel.abilityModelToAbility(resourcesList, this);
        return ability;

    }
}