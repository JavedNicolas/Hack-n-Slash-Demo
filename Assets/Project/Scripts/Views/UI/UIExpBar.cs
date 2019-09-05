using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIExpBar: MonoBehaviour
{
    [SerializeField] Image _expBarFill;
    [SerializeField] TextMeshProUGUI _expPercentage;

    public void updateExpDisplay(float exp)
    {
        _expBarFill.fillAmount = exp;
        _expPercentage.text = Mathf.Ceil(exp * 100).ToString() + "%";
    }
}
