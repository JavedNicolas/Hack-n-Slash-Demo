using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UISkillSlot : MonoBehaviour, IPopUpOnHovering, IPointerDownHandler
{
    [Header("Skill attributs")]
    public bool isChoiceIcon = false;
    public Image skillIcon;
    public GameObject keyDisplayer;
    public string inputName;

    Ability _skill;
    public Ability ability
    {
        get { return _skill; }
        set
        {
            _skill = value;
            setIcon(_skill);
        }
    }

    // skill choice
    UISkillChoice _skillChoice;

    private void Start()
    {
        skillIcon.enabled = false;
    }

    public void init(UISkillChoice skillChoice)
    {
        this._skillChoice = skillChoice;
    }

    private void Update()
    {
        if (ability != null && !skillIcon.enabled)
            skillIcon.enabled = true;
    }

    public void setSkillSlot(bool isChoiceIcon,  Ability ability, Transform parent)
    {
        if (isChoiceIcon)
            keyDisplayer.SetActive(false);

        this.isChoiceIcon = isChoiceIcon;
        this.ability = ability;
        transform.SetParent(parent);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        displayPopUp(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayPopUp(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (isChoiceIcon)
            {
                GameManager.instance.lockClick(true, true);
                _skillChoice.updateSkillWithChoice(_skill);
                _skillChoice.gameObject.SetActive(false);
                displayPopUp(false);
                GameManager.instance.lockClick(false, true);
            }
            else
            {
                GameObject choiceObject = _skillChoice.gameObject;

                if (_skillChoice.skillSlotToChange == this)
                    choiceObject.SetActive(!choiceObject.activeSelf);
                else
                {
                    _skillChoice.displaySkills(this);
                    choiceObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Set the icon of the ability
    /// </summary>
    /// <param name="ability"></param>
    void setIcon(Ability ability)
    {
        skillIcon.sprite = ability.getIcon();
        skillIcon.enabled = true;
    }

    public void displayPopUp(bool display)
    {
        if(ability != null)
        {
            GameUI.instance.displayDescription(display, ability, this);
        }
    }
}
