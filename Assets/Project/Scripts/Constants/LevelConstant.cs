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
        100, // Level 1 to 2
        200,  // Level 2 to 3
        350,  // Level 3 to 4
        700,  // Level 4 to 5
        1400,  // Level 5 to 6
        2400,  // Level 6 to 7
        4000,  // Level 7 to 8
        8000,  // Level 8 to 9
        15000,  // Level 9 to 10
        20000,  // Level 10 to 11
        25000,  // Level 11 to 12
        30000,  // Level 12 to 13
        35000,  // Level 13 to 14
        40000,  // Level 14 to 15
        45000,  // Level 15 to 16
        50000,  // Level 16 to 17
        55000,  // Level 17 to 18
        60000,  // Level 18 to 19
        65000,   // Level 18 to 19
        70000   // Level 19 to 20
    };
}
