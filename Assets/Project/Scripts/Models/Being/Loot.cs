using UnityEngine;
using System.Collections;

public struct Loot 
{
    [SerializeField] public Item item;
    [SerializeField] public float changeToDrop;
    [SerializeField] public int quantity;

    public Loot(Item item, float changeToDrop, int quantity)
    {
        this.item = item;
        this.changeToDrop = changeToDrop;
        this.quantity = quantity;
    }
}
