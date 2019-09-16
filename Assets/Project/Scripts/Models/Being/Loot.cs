using UnityEngine;
using System.Collections;

[System.Serializable]
public class Loot 
{
    public Item item = null;
    [SerializeField] int itemID;
    [SerializeField] [Range(0,100)] public float changeToDrop;
    [SerializeField] public int quantity;
    [SerializeField] public bool isRandom;

    /// <summary>
    /// used to generate loot
    /// </summary>
    /// <param name="loot"></param>
    public Loot(Loot loot)
    {
        this.itemID = loot.item.databaseID;
        if (itemID == -1)
            getRandomItem();
        else
        {
            this.quantity = loot.quantity;
            this.item = new Item(GameManager.instance.itemDatabase.getElementWithDBID(itemID));
        }
           
        this.changeToDrop = loot.changeToDrop;
        this.isRandom = loot.isRandom;
    }

    /// <summary>
    /// Use for database editor
    /// </summary>
    /// <param name="item"></param>
    /// <param name="changeToDrop"></param>
    /// <param name="quantity"></param>
    public Loot(Item item, float changeToDrop, int quantity)
    {
        if (item != null)
            this.itemID = item.databaseID;
        this.changeToDrop = changeToDrop;
        this.quantity = quantity;
    }

    void getRandomItem()
    {
        item = new Item(GameManager.instance.itemDatabase.getRandomElement());
        itemID = item.databaseID;
        quantity = 1;
    }
}
