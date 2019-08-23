using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatabaseElement
{
    string _name;
    [SerializeField] public string name { get => _name; set => _name = value; }
    public int _databaseID;
    public int databaseID { get => _databaseID; set => _databaseID = value; }
}
