using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ScriptableObjectConstant.effectMenuName + "Recycle Effect", fileName = "Recycle Effect")]
public class RecycleEffect : Effect
{
    public override void effect(BeingBehavior sender, GameObject target, float value)
    {
        InventorySlotUI inventorySlotUI = target.GetComponent<InventorySlotUI>();
        if (inventorySlotUI != null)
        {
            InventorySlot slot = inventorySlotUI.getInventorySlot();
            slot.removeItem();
            GameUI.instance.updateInventorySlots(new List<InventorySlot>() { slot });
        }
    }
}
