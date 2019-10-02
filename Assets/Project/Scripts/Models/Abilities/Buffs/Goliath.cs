using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goliath : Ability
{
    public override void animation()
    {
       
    }

    public override void performAbility(BeingBehavior sender, BeingBehavior target)
    {
        if(sender.teamID == target.teamID)
        {
            List<Stat> goliathBuffStats = new List<Stat>();
            goliathBuffStats.Add(new Stat(StatType.Life, StatBonusType.additional, 250, "Goliath"));
            goliathBuffStats.Add(new Stat(StatType.AreaSize, StatBonusType.Multiplied, 100, "Goliath"));
            float duration = ((IDurationAttributs)abilityAttributs).duration;
            Buff goliathBuff = new Buff("Goliath", abilityAttributs.icon, duration, goliathBuffStats);

            float lifeBeforeBuff = target.being.stats.maxLife;
            target.being.addBuff(goliathBuff);

            // Heal the target for the life added
            target.being.heal(target.being.stats.maxLife - lifeBeforeBuff);
        }
    }

    public override void performAbility(BeingBehavior sender, Vector3 targetedPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void setBaseStats()
    {
        
    }

}
