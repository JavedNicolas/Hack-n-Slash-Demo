using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UISkillSlot : MonoBehaviour
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
    /// Set the icon of the ability
    /// </summary>
    /// <param name="ability"></param>
    void setIcon(Ability ability)
    {
        GetComponent<Image>().sprite = ability.getIcon();
    }

    /// <summary>
    /// Display The skill choice UI or choose the skill
    /// </summary>
    public void onClick()
    {
        if (isChoiceIcon)
        {
            UISkillChoice.instance.updateSkillWithChoice(_skill);
            UISkillChoice.instance.gameObject.SetActive(false);
        }
        else
        {
            GameObject choiceObject = UISkillChoice.instance.gameObject;

            if(UISkillChoice.instance.skillSlotToChange == this)
                choiceObject.SetActive(!choiceObject.activeSelf);
            else
            {
                UISkillChoice.instance.displaySkills(this);
                choiceObject.SetActive(true);
            }
                

        }

    }
}
