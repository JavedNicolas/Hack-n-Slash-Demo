using UnityEngine;
using System.Collections;

public class PlayerAbilityManager : AbilityManager
{
    // override the being behavior
    protected new PlayerBehavior abilitySender { get => (PlayerBehavior)_abilitySender; }
}
