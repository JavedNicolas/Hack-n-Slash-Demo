using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIAbilityDescriptionPopUp : MonoBehaviour
{
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dpsText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI statsText;

    public void setText(Ability ability, Being player)
    {
        this.nameText.text = ability.getName();
        this.descriptionText.text = ability.getDescription(player);
        this.statsText.text = "";

        List<string> descriptions = new List<string>();
        
        foreach(AbilityEffectAndValue effectAndValue in ability.abilityAttributs.effectAndValues)
        {
            descriptions.Add(effectAndValue.getDescription(player, ability));
        }

        descriptions.AddRange(player.stats.getBonusListFor(ability));

        for (int i = 0; i < descriptions.Count; i++)
        {
            if(descriptions[i] != "")
                statsText.text += descriptions[i] + "\n";
        }
    }

}

