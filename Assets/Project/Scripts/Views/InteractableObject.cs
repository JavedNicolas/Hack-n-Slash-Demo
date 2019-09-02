using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MeshRenderer))]
public abstract class InteractableObject : MonoBehaviour
{
    // Private var
    protected MeshRenderer renderer;

    [Header("Object attributs")]
    public Interactable interactable;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// delete the item from the scene
    /// </summary>
    protected virtual void deleteItem()
    {
        DestroyImmediate(this.gameObject);
    }
}
