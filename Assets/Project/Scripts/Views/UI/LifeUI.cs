using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Image lifeFill;
    public bool isEnemyBar;
    Being being;

    private void Update()
    {
        if(isEnemyBar)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        }
        updateLife();
    }

    public void updateLife()
    {
        if(being != null)
            lifeFill.fillAmount = being.currentLife / being.baseLife;
    }

    public void setBeing(Being being)
    {
        this.being = being;
    }


}
