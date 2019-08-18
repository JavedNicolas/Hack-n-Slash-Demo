using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePotion : Item
{

    public LifePotion()
    {
        itemName = "Life Potion";
        databaseID = 1;
        interactibleType = InteractableObjectType.PickableObject;
    }

    public override void use(Being sender = null, Item target = null)
    {
        base.use(sender, target);
        sender.heal(10);
    }
}
