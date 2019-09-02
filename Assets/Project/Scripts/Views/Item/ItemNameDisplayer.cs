using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemNameDisplayer : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] InteractableObject parentObject;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
            GameManager.instance.GetPlayerBehavior().AddInteractibleTaget(parentObject);
    }
}
