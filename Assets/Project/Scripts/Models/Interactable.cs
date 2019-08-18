using System.Collections;
using UnityEngine;

public abstract class Interactable
{
    float distanceToInteract;

    // true if the movement has been initiated
    bool moveHasStarted = false;
    public InteractableObjectType Interactibletype;

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
    /// <param name="player">The player interacting</param>
    /// <returns>Return true when the movement to the object is over. This movement can be interacted and the result found in the interactionResult attribut</returns>
    public abstract bool interact(PlayerBehavior player);

}
