using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class UISkillBar: MonoBehaviour
{
    [SerializeField] UISkillChoice _skillChoice;
    [SerializeField] List<UISkillSlot> skillSlots = new List<UISkillSlot>();
    GridLayoutGroup gridLayout;

    public void init(Player player)
    {
        _skillChoice.init(player);
        getSkillSlots();
    }

    private void LateUpdate()
    {
        if (skillSlots.Count > 0)
            GetComponent<GridLayoutGroup>()?.setCellSize(Orientation.Horizontal, skillSlots.Count, GetComponent<RectTransform>());
    }

    /// <summary>
    /// get all the skill slot in child
    /// </summary>
    void getSkillSlots()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UISkillSlot skillSlot;
            if (transform.GetChild(i).TryGetComponent(out skillSlot))
            {
                skillSlots.Add(skillSlot);
                skillSlot.init(_skillChoice);
            }  
        }
    }

    /// <summary>
    /// Get the number of skill slot available
    /// </summary>
    /// <returns></returns>
    public int getSkillSlotNumber()
    {
        return skillSlots.Count;
    }

    /// <summary>
    /// Return the skill corresponding to the inputName
    /// </summary>
    /// <param name="inputName">The name of the key in the input table</param>
    /// <returns></returns>
    public UISkillSlot getSkillAtIndex(int index)
    {
        if (index > skillSlots.Count)
            return null;

        return skillSlots[index];
    }
}
