using UnityEngine;
using System.Collections;

public class PlayerAbilityManager : AbilityManager
{
    // override the being behavior
    protected new PlayerBehavior abilitySender { get => (PlayerBehavior)_abilitySender; }

    protected override bool tryPerformNotTargetedAbility(Vector3 targetedPosition, Ability ability)
    {
        if (checkMana(ability))
        {
            ability.performAbility(abilitySender, targetedPosition);
            abilitySender.being.spendMana(ability.abilityAttributs.manaCost);
            return true;
        }
        return false;
    }

}
