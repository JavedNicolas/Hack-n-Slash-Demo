using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePotion : Item
{

    public LifePotion()
    {
        itemName = "Life Potion";
    }

    public override void effect(Being sender = null, Item target = null)
    {
        base.effect(sender, target);
        sender.heal(10);
    }
}
