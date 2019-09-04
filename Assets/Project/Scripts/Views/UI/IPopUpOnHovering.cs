using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface IPopUpOnHovering : IPointerEnterHandler, IPointerExitHandler
{
    void displayPopUp(bool display);
}
