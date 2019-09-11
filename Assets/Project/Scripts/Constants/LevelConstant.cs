using UnityEngine;
using System.Collections;

public enum LevelType
{
    Base, Job, Ability
}

public static class LevelExperienceTable
{
    public static float[] levelExperienceNeeded = new float[]
    {
        0,
        100, // Level 2
        200,  // Level 3
        350,  // Level 4
        700,  // Level 5
        800  // Level 6
    };
}
