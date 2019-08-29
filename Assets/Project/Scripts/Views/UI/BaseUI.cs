using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Movable attributs")]
    [SerializeField] protected bool _canBeUsedToMove = false;
    [SerializeField] protected GameObject _objectToMove;

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.1f;
    bool DoubleClickHasBeenFired = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            rightClickOnUI();
        else if(eventData.button == PointerEventData.InputButton.Left)
            leftClickOnUI();
    }

    protected abstract void leftClickOnUI();
    protected abstract void rightClickOnUI();
}
