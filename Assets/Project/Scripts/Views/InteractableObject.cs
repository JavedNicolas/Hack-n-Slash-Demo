using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MeshRenderer))]
public abstract class InteractableObject : MonoBehaviour
{
    // Private var
    protected MeshRenderer mesRenderer;

    [Header("Object attributs")]
    public Interactable interactable;

    private void Awake()
    {
        mesRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// delete the item from the scene
    /// </summary>
    protected virtual void deleteItem()
    {
        DestroyImmediate(this.gameObject);
    }
}
