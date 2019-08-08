using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChoiceUI : MonoBehaviour
{
    [Header("Skill icon prefab")]
    public GameObject skillIconUIPrefab;

    // instance 
    public static SkillChoiceUI instance;

    private SkillSlotUI _skillSlotToChange;
    public SkillSlotUI skillSlotToChange {  get { return _skillSlotToChange; } }

    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Show all the skill the player have
    /// </summary>
    public void displaySkills(SkillSlotUI forSkillSlot)
    {
        this._skillSlotToChange = forSkillSlot;

        clearChild();
        Player player = GameManager.instance.getPlayer();

        for (int i = 0; i < player.skills.Count; i++)
        {
            GameObject skillIcon = Instantiate(skillIconUIPrefab);
            SkillSlotUI skillIconUI = skillIcon.GetComponent<SkillSlotUI>();
            if (skillIconUI != null)
            {
                skillIconUI.skill = player.skills[i];
                skillIconUI.isChoiceIcon = true;
                skillIcon.transform.SetParent(transform);
            }

        }
    }

    /// <summary>
    /// Update the skill which was at the origin of the choice pop up by the user choice
    /// </summary>
    /// <param name="skill">The skill choosen</param>
    public void updateSkillWithChoice(Skill skill)
    {
        _skillSlotToChange.skill = skill;
    }

    /// <summary>
    /// Clear child to avoir duplicate
    /// </summary>
    void clearChild()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
