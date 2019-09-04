using UnityEngine;
using TMPro;
using System.Collections;

public class UIDescriptionPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMPro;
    [SerializeField] TextMeshProUGUI descriptionTMPro;

    public void setText(IDescribable describable)
    {
        this.nameTMPro.text = describable.getName() ;
        this.descriptionTMPro.text = describable.getDescription();
    }
}
