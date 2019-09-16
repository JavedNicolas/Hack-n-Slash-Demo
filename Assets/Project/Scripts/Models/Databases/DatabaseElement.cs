using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatabaseElement : IDescribable
{
    [SerializeField] public string name;
    [SerializeField] public int databaseID = -1;

    public virtual string getName()
    {
        return name;
    }

    public virtual string getDescription(Being owner)
    {
        return "";
    }
}
