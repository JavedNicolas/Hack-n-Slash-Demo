using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIAbilityDescriptionPopUp : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI nameTMPro;
    [SerializeField] TextMeshProUGUI dps;
    [SerializeField] TextMeshProUGUI descriptionTMPro;
    [SerializeField] TextMeshProUGUI statsTMPro;

    public void setText(Ability ability, Being player)
    {
        this.nameTMPro.text = ability.getName();
        this.descriptionTMPro.text = ability.getDescription(player);
    }
}

