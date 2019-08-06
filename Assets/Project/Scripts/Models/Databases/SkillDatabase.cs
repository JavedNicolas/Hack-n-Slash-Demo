using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : Database<Skill>
{
    public SkillDatabase()
    {
        elements = new List<Skill>()
        {
            new BasicAttack(),
            new LightningBall()
        };
    }
}
