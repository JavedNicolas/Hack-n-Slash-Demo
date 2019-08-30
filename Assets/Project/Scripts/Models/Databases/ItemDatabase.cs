using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = DatabaseConstant.itemDatabasePath, menuName = "Database/Item")]
public class ItemDatabase : Database<Item>
{
}
