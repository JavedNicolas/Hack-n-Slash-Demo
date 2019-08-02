using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    // instance
    public static GameUI instance;

    [Header("Base Screen UI")]
    public GameObject lifeUI;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        
    }

    public void displayUI(Player player)
    {
        lifeUI.GetComponent<LifeUI>().setBeing(player);
    }

    public void FixedUpdate()
    {

    }


}
