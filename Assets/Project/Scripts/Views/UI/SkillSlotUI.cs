using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class SkillSlotUI : MonoBehaviour
{
    Skill _skill;
    public Skill skill
    {
        get { return _skill; }
        set {
            _skill = value;
            setIcon(_skill);
        }
    }

    public bool isChoiceIcon = false;
    public string inputName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(onClick);
        //GetComponentInChildren<TextMeshProUGUI>().text = 
    }

    /// <summary>
    /// Set the icon of the skill
    /// </summary>
    /// <param name="skill"></param>
    void setIcon(Skill skill)
    {
        if(isChoiceIcon)
            GetComponent<Image>().sprite = skill.icon;
    }

    /// <summary>
    /// Display The skill choice UI or choose the skill
    /// </summary>
    public void onClick()
    {
        if (isChoiceIcon)
        {
            SkillChoiceUI.instance.updateSkillWithChoice(_skill);
            SkillChoiceUI.instance.gameObject.SetActive(false);
        }
        else
        {
            GameObject choiceObject = SkillChoiceUI.instance.gameObject;
            choiceObject.SetActive(!choiceObject.activeSelf);
            SkillChoiceUI.instance.displaySkills(this);
        }

    }
}
