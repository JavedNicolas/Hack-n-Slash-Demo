using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DatabaseElement
{
    [SerializeField] protected string _name;
    public string name { get => _name; set => _name = value; }
    [SerializeField] protected int _databaseID;
    public int databaseID { get => _databaseID; set => _databaseID = value; }
}
