using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PassiveAvailableButton : MonoBehaviour
{
    GameManager GameManagerInstance;
    Button button;

    // Use this for initialization
    void Start()
    {
        button = gameObject.GetComponentInChildren<Button>();
        button?.onClick.AddListener(displayPassiveTree);
        button?.gameObject.SetActive(false);
        GameManagerInstance = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerInstance.playerHasPassivePointAvailable())
            button?.gameObject.SetActive(true);
        else
            button?.gameObject.SetActive(false);
    }

    void displayPassiveTree()
    {
        UIPassiveTree.instance.displayTree();
    }
}
