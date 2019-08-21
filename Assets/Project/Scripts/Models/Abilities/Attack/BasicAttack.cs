using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{
    public BasicAttack() : base(Resources.Load<BasicAttackAttributs>(AbilityConstant.attributsFolder + "BasicAttackAttributs"))
    {
    }

    public override void animation()
    {
        throw new System.NotImplementedException();
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior targetGameObject)
    {
        BasicAttackAttributs attributs = (BasicAttackAttributs)abilityAttributs;
        //targetGameObject.being.takeDamage(attributs.value);
    }

    public override void performAbility(BeingBehavior sender, Vector3 targedPosition, BeingBehavior senderGameObject)
    {
        throw new System.NotImplementedException();
    }
}
