using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using System;

[CreateAssetMenu(menuName = "Database/Ability", fileName = DatabaseConstant.abilityDatabaseFileName)]
public class AbilityDatabase : Database<Ability>
{
    [SerializeField] public List<Attribut> attributs = new List<Attribut>();

    private void OnEnable()
    {
        elements = AbilityList.list;
        loadDB();
    }

    void setAttributs()
    {
        updateAttributsSize();

        for(int i =0; i < elements.Count; i++)
        {
            if(attributs.Exists(x => x.abilityName == elements[i].getName()))
            {
                int index = attributs.FindIndex(x => x.abilityName == elements[i].getName());
                elements[i].databaseID = attributs[index].id;
                elements[i].abilityAttributs = attributs[index].abilityAttributs;
            }
            else
            {
                if(attributs.Exists(x => x.abilityName == "")){
                    int index = attributs.FindIndex(x => x.abilityName == "");
                    attributs[index].abilityName = elements[i].getName();
                    elements[i].databaseID = attributs[index].id;
                }
            }
            
        }
        orderDB();
    }

    void updateAttributsSize()
    {
        while (attributs.Count != elements.Count)
        {
            int freeId = getFreeId();
            if (attributs.Count < elements.Count)
                attributs.Add(new Attribut("", null, freeId));
            if (attributs.Count > elements.Count)
                attributs.RemoveAt(attributs.Count - 1);
        }
    }

    public override int getFreeId()
    {
        for (int i = 0; i < attributs.Count; i++)
        {
            if(!attributs.Exists(x => x.id == i))
            {
                return i + 1;
            }
        }
        return attributs.Count;
    }

    public override void updateElementAt(Ability element, int index)
    {

        base.updateElementAt(element, index);
        attributs[index] = new Attribut(element.getName(), element.abilityAttributs, element.databaseID);
    }

    /// <summary>
    /// Save the db in the json file
    /// </summary>
    public override void saveDB()
    {
        JsonWrappingClass<Attribut> wrapingClass = new JsonWrappingClass<Attribut>(attributs);
        string dbAsString = JsonUtility.ToJson(wrapingClass);
        File.WriteAllText(AssetDatabase.GetAssetPath(_file), dbAsString);
    }

    /// <summary>
    ///  load the Db from the json file
    /// </summary>
    public override void loadDB()
    {
        elements = AbilityList.list;
        attributs = new List<Attribut>();

        string dbAsString = File.ReadAllText(AssetDatabase.GetAssetPath(_file));
        if (dbAsString != "")
            attributs = JsonUtility.FromJson<JsonWrappingClass<Attribut>>(dbAsString)._elements;
        else
            attributs = new List<Attribut>();

        setAttributs();
    }
}

[System.Serializable]
public class Attribut
{
    [SerializeField] public string abilityName;
    [SerializeField] public AbilityAttributs abilityAttributs;
    [SerializeField] public int id = -1;

    public Attribut(string abilityName, AbilityAttributs abilityAttributs, int id)
    {
        this.abilityName = abilityName;
        this.abilityAttributs = abilityAttributs;
        this.id = id;
    }
}
