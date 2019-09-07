using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMana : MonoBehaviour
{
    public Image manaFill;
    public bool isSmallManaBar;
    public TextMeshProUGUI manaAsText;
    Player player;

    private void Update()
    {
        if(isSmallManaBar)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        }
        updateMana();
    }

    public void updateMana()
    {
        if(player != null)
        {
            manaFill.fillAmount = player.currentMana / player.maxMana;
            if (!isSmallManaBar)
                manaAsText.text = player.currentMana.ToString() + "/" + player.maxMana.ToString();
        }
        
    }

    public void setBeing(Player being)
    {
        this.player = being;
    }


}
