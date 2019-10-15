using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecycleEffect : Effect
{
    protected override string description => "Recycle_Description".localize();

    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        UIInventorySlot inventorySlotUI = target.GetComponent<UIInventorySlot>();
        if (inventorySlotUI != null && inventorySlotUI.hasItem())
            if (inventorySlotUI.inventorySlot.item.canBeRecycle)
                return true;

        return false;
    }

    public override void use(BeingBehavior sender, GameObject target, float value, DatabaseElement effectOrigin = null)
    {
        if (canBeUsed(sender, target, value))
        {
            UIInventorySlot inventorySlotUI = target.GetComponent<UIInventorySlot>();
            InventorySlot slot = inventorySlotUI.getInventorySlot();
            slot.removeItem();
            inventorySlotUI.updateInventorySlots(new List<InventorySlot>() { slot });
        }
    }
}
