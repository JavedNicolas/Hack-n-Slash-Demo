using UnityEngine;
using System.Collections;

public class EnemyAbilityManager : AbilityManager
{
    // override the being behavior
    protected new EnemyBehavior abilitySender { get => (EnemyBehavior)_abilitySender; }
}
