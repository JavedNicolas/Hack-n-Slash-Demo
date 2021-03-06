﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillChoice : MonoBehaviour
{
    [Header("Skill icon prefab")]
    public GameObject skillIconUIPrefab;
    [Header("Number of slot per Line")]
    public int numberOfSlotPerLine;

    Player _player;

    private UISkillSlot _skillSlotToChange;
    public UISkillSlot skillSlotToChange {  get { return _skillSlotToChange; } }

    public void init(Player player)
    {
        this._player = player;
    }

    /// <summary>
    /// Show all the skill the player have
    /// </summary>
    public void displaySkills(UISkillSlot forSkillSlot)
    {
        this._skillSlotToChange = forSkillSlot;

        transform.clearChild();
        GetComponent<GridLayoutGroup>()?.setCellSize(Orientation.Horizontal, numberOfSlotPerLine, GetComponent<RectTransform>());

        for (int i = 0; i < _player?.abilities.Count; i++)
        {
            GameObject skillIcon = Instantiate(skillIconUIPrefab);
            UISkillSlot skillIconUI = skillIcon.GetComponent<UISkillSlot>();
            if (skillIconUI != null)
            {
                skillIconUI.init(this);
                skillIconUI.setSkillSlot(true, _player?.abilities[i], transform);
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
}
