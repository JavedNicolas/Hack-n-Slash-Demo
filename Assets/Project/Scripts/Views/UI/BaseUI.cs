using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public abstract class BaseUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{

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

    public void OnDrag(PointerEventData eventData)
    {
        dragging(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragginEnd(eventData);
    }

    public virtual Vector3 getDescriptionPopUpOffset()
    {
        float offsetX = GetComponent<RectTransform>().sizeDelta.x;
        float offsetY = GetComponent<RectTransform>().sizeDelta.y;
        return new Vector3(offsetX, offsetY);
    }

    public float getWidthSize()
    {
        return GetComponent<RectTransform>().sizeDelta.x;
    }

    public float getHeightSize()
    {
        return GetComponent<RectTransform>().sizeDelta.y;
    }

    protected abstract void leftClickOnUI();
    protected abstract void rightClickOnUI();

    protected abstract void dragging(PointerEventData eventData);
    protected abstract void dragginEnd(PointerEventData eventData);


}
