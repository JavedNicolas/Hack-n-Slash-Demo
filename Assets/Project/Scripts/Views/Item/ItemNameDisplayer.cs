using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemNameDisplayer : BaseUI, IPointerDownHandler, IPopUpOnHovering
{
    [SerializeField] InteractableObject parentObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        displayPopUp(true);   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayPopUp(false);
    }

    public void displayPopUp(bool display)
    {
        ItemObject itemObject = parentObject.GetComponent<ItemObject>();
        if (itemObject != null)
            GameUI.instance.displayDescription(display, itemObject.loot.item, this, true);
    }

    #region base ui functions
    protected override void leftClickOnUI()
    {
        
    }

    protected override void rightClickOnUI()
    {
        GameManager.instance.GetPlayerBehavior().AddInteractibleTaget(parentObject);
    }

    protected override void dragging(PointerEventData eventData)
    {
        
    }

    protected override void dragginEnd(PointerEventData eventData)
    {
        
    }
    #endregion
}
