using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Recycle Effect", fileName = "Recycle Effect")]
public class RecycleEffect : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        InventorySlotUI inventorySlotUI = target.GetComponent<InventorySlotUI>();
        if (inventorySlotUI != null && inventorySlotUI.hasItem())
            if (inventorySlotUI.inventorySlot.item.canBeRecycle)
                return true;

        return false;
    }

    public override void use(BeingBehavior sender, GameObject target, float value)
    {
        if (canBeUsed(sender, target, value))
        {
            InventorySlotUI inventorySlotUI = target.GetComponent<InventorySlotUI>();
            InventorySlot slot = inventorySlotUI.getInventorySlot();
            slot.removeItem();
            GameUI.instance.updateInventorySlots(new List<InventorySlot>() { slot });
        }
    }
}
