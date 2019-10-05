using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

[System.Serializable]
public class Database<T> : ScriptableObject where T : DatabaseElement
{
    [SerializeField] protected Object _file;
    public Object file { get => _file; }

    [SerializeField] public List<T> elements = new List<T>();

    private void OnEnable()
    {
        orderDB();
    }

    protected void orderDB()
    {
        if(elements.Count > 0)
            elements = elements.OrderBy(x => x.databaseID).ToList();
    }

    public T getElementAt(int index)
    {
        orderDB();
        if (index >= elements.Count)
            return default(T);
        else
            return elements[index];
    }

    public virtual T getElementWithDBID(int id)
    {
        orderDB();
        return elements.Find(x => x.databaseID == id);  
    }

    public T getRandomElement()
    {
        return elements[Random.Range(0, elements.Count)];
    }

    public int getDatabaseSize()
    {
        return elements.Count;
    }

    public void addElement(T element)
    {
        elements.Add(element);
    }

    public void removeElement(T element)
    {
        orderDB();
        elements.Remove(element);
    }

    public virtual void updateElementAt(T element, int index)
    {
        orderDB();
        elements[index] = element;
    }

    public virtual int getFreeId()
    {
        orderDB();
        int lastID = -1;

        if(elements.Find(x => x.databaseID == 0) == null)
        { return 0; }

        for (int i = 0; i < elements.Count; i++)
        {
            int currentID = elements[i].databaseID;
            if (currentID > lastID +1)
                return lastID + 1;
            lastID = currentID;
        }

        return elements.Count;
    }

    /// <summary>
    /// Save the db in the json file
    /// </summary>
    public virtual void saveDB()
    {
        JsonWrappingClass<T> wrapingClass = new JsonWrappingClass<T>(elements);
        string dbAsString = JsonUtility.ToJson(wrapingClass);
        File.WriteAllText(AssetDatabase.GetAssetPath(_file), dbAsString);
    }

    /// <summary>
    ///  load the Db from the json file
    /// </summary>
    public virtual void loadDB()
    {
        string dbAsString = File.ReadAllText(AssetDatabase.GetAssetPath(_file));
        if (dbAsString != "")
            elements = JsonUtility.FromJson<JsonWrappingClass<T>>(dbAsString).elements;
        else
            elements = new List<T>();
    }
}

