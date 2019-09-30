using UnityEngine;
using System.Collections;

[System.Serializable]
public class Loot 
{
    public Item item = null;
    [SerializeField] public int itemDatabaseID = -1;
    [SerializeField] [Range(0,100)] public float chanceToDrop;
    [SerializeField] public int quantity;
    [SerializeField] public bool isRandom;

    /// <summary>
    /// used to generate loot
    /// </summary>
    /// <param name="loot"></param>
    public Loot(Loot loot)
    {
        if (loot.itemDatabaseID == -1 || loot.item == null || isRandom)
            getRandomItem();
        else
        {
            this.quantity = loot.quantity;
            ItemDatabaseModel itemDatabaseModel = GameManager.instance.itemDatabase.getElementWithDBID(loot.item.databaseID);
            this.item = new Item(itemDatabaseModel.databaseModelToItem(GameManager.instance.resourcesList));
            this.itemDatabaseID = loot.item.databaseID;
        }

        this.chanceToDrop = loot.chanceToDrop;
        this.quantity = loot.quantity == 0 ? 1 : loot.quantity;  
        this.isRandom = loot.isRandom;
    }

    /// <summary>
    /// Use for database editor
    /// </summary>
    /// <param name="item"></param>
    /// <param name="chanceToDrop"></param>
    /// <param name="quantity"></param>
    public Loot(Item item, float chanceToDrop, int quantity, bool isRandom = false)
    {
        if (item != null)
        {
            this.itemDatabaseID = item.databaseID;
            this.item = item;
        }

        this.chanceToDrop = chanceToDrop;
        this.quantity = quantity;
        this.isRandom = isRandom;
    }

    /// <summary>
    /// Generate a random Loot
    /// </summary>
    public Loot(float changeToDrop = 100, int quantity = 1)
    {
        getRandomItem();
        this.chanceToDrop = changeToDrop;
        this.quantity = quantity;
        this.isRandom = true;
    }

    void getRandomItem()
    {
        ItemDatabaseModel itemDatabaseModel = GameManager.instance.itemDatabase.getRandomElement();
        this.item = new Item(itemDatabaseModel.databaseModelToItem(GameManager.instance.resourcesList));
        itemDatabaseID = item.databaseID;
    }
}
