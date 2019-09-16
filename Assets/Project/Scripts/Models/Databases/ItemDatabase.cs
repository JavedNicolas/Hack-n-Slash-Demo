using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = DatabaseConstant.itemDatabasePath, menuName = "Database/Item")]
public class ItemDatabase : Database<Item>
{

}
