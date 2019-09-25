using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class Interactable : DatabaseElement
{
    float distanceToInteract;

    // true if the movement has been initiated
    [SerializeField] protected InteractableObjectType _interactibleType;
    public InteractableObjectType interactibleType { get => _interactibleType; set => _interactibleType = value; }

    public virtual float getInteractionDistance()
    {
        return distanceToInteract;
    }

    protected virtual void setDistanceToInteraction(float distance)
    {
        distanceToInteract = distance;
    }

    /// <summary>
    /// Move to the player interacting with the object to the interaction range
    /// </summary>
    /// <param name="interactionBeing">The player interacting</param>
    /// <returns>Return true when the movement to the object is over. This movement can be interacted and the result found in the interactionResult attribut</returns>
    public abstract bool interact(BeingBehavior interactionBeing, GameObject objectToInteractWith);


}
