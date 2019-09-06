using UnityEngine;
using TMPro;
using System.Collections;

public class UIDescriptionPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMPro;
    [SerializeField] TextMeshProUGUI descriptionTMPro;

    public void setText(IDescribable describable, Being player)
    {
        this.nameTMPro.text = describable.getName() ;
        this.descriptionTMPro.text = describable.getDescription(player);
    }
}
