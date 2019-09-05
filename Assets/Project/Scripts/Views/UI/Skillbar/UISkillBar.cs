using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBar: MonoBehaviour
{

    public List<UISkillSlot> numberOfSkillSlot = new List<UISkillSlot>();

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
    public UISkillSlot getSkillAtIndex(int index)
    {
        if (index > numberOfSkillSlot.Count)
            return null;

        return numberOfSkillSlot[index];
    }
}
