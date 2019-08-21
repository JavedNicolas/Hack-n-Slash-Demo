using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : Database<Ability>
{
    public SkillDatabase()
    {
        elements = new List<Ability>()
        {
            new BasicAttack(),
            new LightningBall()
        };
    }
}
