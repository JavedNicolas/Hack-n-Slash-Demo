using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarUI: MonoBehaviour
{
    public static SkillBarUI instance;

    public List<SkillSlotUI> numberOfSkillSlot = new List<SkillSlotUI>();

    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Get the number of skill slot available
    /// </summary>
    /// <returns></returns>
    public int getSkillSlotNumber()
    {

        return numberOfSkillSlot.Count;
    }

    /// <summary>
    /// Return the skill corresponding to the inputName
    /// </summary>
    /// <param name="inputName">The name of the key in the input table</param>
    /// <returns></returns>
    public SkillSlotUI getSkillAtIndex(int index)
    {
        if (index > numberOfSkillSlot.Count - 1)
            return null;

        return numberOfSkillSlot[index];
    }
}
