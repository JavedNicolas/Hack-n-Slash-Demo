using UnityEngine;
using System.Collections;

[System.Serializable]
public class Loot 
{
    [SerializeField] public Item item;
    [SerializeField] [Range(0,100)] public float changeToDrop;
    [SerializeField] public int quantity;
    [SerializeField] public bool isRandom;

    public Loot(Loot loot)
    {
        this.item = new Item(loot.item);
        this.changeToDrop = loot.changeToDrop;
        this.quantity = loot.quantity;
        this.isRandom = loot.isRandom;
    }

    public Loot(Item item, float changeToDrop, int quantity)
    {
        if(item != null)
            this.item = new Item(item);
        this.changeToDrop = changeToDrop;
        this.quantity = quantity;
    }

    public void getRandomItem()
    {
        item = new Item(GameManager.instance.itemDatabase.getRandomElement());
        quantity = 1;
    }
}
