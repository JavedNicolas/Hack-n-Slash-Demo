using UnityEngine;
using System.Collections;

[System.Serializable]
public class LootDatabaseModel 
{
    [SerializeField] public int itemDatabaseID;
    [SerializeField] public float chanceToDrop;
    [SerializeField] public int quantity;
    [SerializeField] public bool isRandom;

    public LootDatabaseModel(int itemDatabaseID, float chanceToDrop, int quantity, bool isRandom)
    {
        this.itemDatabaseID = itemDatabaseID;
        this.chanceToDrop = chanceToDrop;
        this.quantity = quantity;
        this.isRandom = isRandom;
    }


    public Loot databaseModelToLoot(DatabaseResourcesList resourcesList, ItemDatabase itemDatabase)
    {
        Item item = itemDatabase.getElementWithDBID(itemDatabaseID)?.databaseModelToItem(resourcesList);
        return new Loot(item, chanceToDrop, quantity, isRandom);
    }
}
