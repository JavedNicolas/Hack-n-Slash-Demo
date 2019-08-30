using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILife : MonoBehaviour
{
    public Image lifeFill;
    public bool isSmallLifeBar;
    public TextMeshProUGUI lifeAsText;
    Being being;

    private void Update()
    {
        if(isSmallLifeBar)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        }
        updateLife();
    }

    public void updateLife()
    {
        if(being != null)
        {
            lifeFill.fillAmount = being.currentLife / being.baseLife;
            if (!isSmallLifeBar)
                lifeAsText.text = being.currentLife.ToString() + "/" + being.baseLife.ToString();
        }
        
    }

    public void setBeing(Being being)
    {
        this.being = being;
    }


}
