using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillChoice : MonoBehaviour
{
    [Header("Skill icon prefab")]
    public GameObject skillIconUIPrefab;

    // instance 
    public static UISkillChoice instance;

    private UISkillSlot _skillSlotToChange;
    public UISkillSlot skillSlotToChange {  get { return _skillSlotToChange; } }

    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Show all the skill the player have
    /// </summary>
    public void displaySkills(UISkillSlot forSkillSlot)
    {
        this._skillSlotToChange = forSkillSlot;

        clearChild();
        Player player = GameManager.instance.getPlayer();

        for (int i = 0; i < player.ability.Count; i++)
        {
            GameObject skillIcon = Instantiate(skillIconUIPrefab);
            UISkillSlot skillIconUI = skillIcon.GetComponent<UISkillSlot>();
            if (skillIconUI != null)
            {
                skillIconUI.ability = player.ability[i];
                skillIconUI.isChoiceIcon = true;
                skillIcon.transform.SetParent(transform);
                skillIconUI.transform.localScale = new Vector3(1, 1, 1);
            }

        }
    }

    /// <summary>
    /// Update the skill which was at the origin of the choice pop up by the user choice
    /// </summary>
    /// <param name="skill">The skill choosen</param>
    public void updateSkillWithChoice(Ability skill)
    {
        _skillSlotToChange.ability = skill;
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
