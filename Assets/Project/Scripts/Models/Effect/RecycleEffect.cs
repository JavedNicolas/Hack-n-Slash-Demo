using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Recycle Effect", fileName = "Recycle Effect")]
public class RecycleEffect : Effect
{
    public override bool canBeUsed(BeingBehavior sender, GameObject target, float value)
    {
        UIInventorySlot inventorySlotUI = target.GetComponent<UIInventorySlot>();
        Debug.Log((inventorySlotUI != null) + " : " + inventorySlotUI.hasItem() + " : " + inventorySlotUI.inventorySlot.item.canBeRecycle);
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
