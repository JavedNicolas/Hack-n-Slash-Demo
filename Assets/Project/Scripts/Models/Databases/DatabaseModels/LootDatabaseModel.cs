using UnityEngine;
using System.Collections;

[System.Serializable]
public class LootDatabaseModel 
{
    [SerializeField] public int itemDatabaseID;
    [SerializeField] public float chanceToDrop;
    [SerializeField] public int quantity;
    [SerializeField] public bool isRandom;

    public LootDatabaseModel(Loot loot)
    {
        this.itemDatabaseID = loot.itemDatabaseID;
        this.chanceToDrop = loot.chanceToDrop;
        this.quantity = loot.quantity;
        this.isRandom = loot.isRandom;
    }


    public Loot databaseModelToLoot(ResourcesList resourcesList, ItemDatabase itemDatabase)
    {
        Item item = itemDatabase.getElementWithDBID(itemDatabaseID)?.databaseModelToItem(resourcesList);
        return new Loot(item, chanceToDrop, quantity, isRandom);
    }
}
