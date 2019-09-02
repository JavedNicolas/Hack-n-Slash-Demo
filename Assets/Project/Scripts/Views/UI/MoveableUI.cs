using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MoveableUI : BaseUI
{
    [Header("Movable attributs")]
    [SerializeField] protected bool _canBeUsedToMove = false;
    [SerializeField] protected GameObject _objectToMove;

    protected override void dragginEnd(PointerEventData eventData)
    {
        
    }

    protected override void dragging(PointerEventData eventData)
    {
        
    }

    protected override void leftClickOnUI()
    {
        
    }

    protected override void rightClickOnUI()
    {
        
    }
}
