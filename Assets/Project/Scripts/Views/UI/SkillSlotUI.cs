using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class SkillSlotUI : MonoBehaviour
{
    Ability _skill;
    public Ability ability
    {
        get { return _skill; }
        set {
            _skill = value;
            setIcon(_skill);
        }
    }

    public bool isChoiceIcon = false;
    public GameObject keyDisplayer;
    public string inputName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(onClick);
        //GetComponentInChildren<TextMeshProUGUI>().text = 
    }

    private void Start()
    {
        if (isChoiceIcon)
            keyDisplayer.SetActive(false);
    }

    /// <summary>
    /// Set the icon of the skill
    /// </summary>
    /// <param name="skill"></param>
    void setIcon(Ability skill)
    {
        if(isChoiceIcon)
            GetComponent<Image>().sprite = skill.getIcon();
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

            if(SkillChoiceUI.instance.skillSlotToChange == this)
                choiceObject.SetActive(!choiceObject.activeSelf);
            else
            {
                SkillChoiceUI.instance.displaySkills(this);
                choiceObject.SetActive(true);
            }
                

        }

    }
}
