using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // instance
    public static GameUI instance;

    [Header("Player UI")]
    [SerializeField] public UIInventory inventoryUI;
    [SerializeField] public UISkillBar skillBar;
    [SerializeField] public UILife lifeUI;
    [SerializeField] public UIMana manaUI;
    [SerializeField] public UITopBar topBar;
    [SerializeField] UILevelUpDisplayer levelUpDisplayer;

    [Header("Description")]
    [SerializeField] public Canvas detailPopUPCanvas;
    [SerializeField] public UIBasicDescriptionPopUp detailPopUpPrefab;
    [SerializeField] public UIAbilityDescriptionPopUp abilityPopUpPrefab;

    UIBasicDescriptionPopUp detailPopUp;
    UIAbilityDescriptionPopUp abilityDetailPopUp;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void loadUI()
    {
        StartCoroutine("loadPassiveTree");
        initDetailPopUp();
    }

    IEnumerator loadPassiveTree()
    {
        SceneManager.LoadScene(SceneConstant.passiveTreeSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => UIPassiveTree.instance != null);
        UIPassiveTree.instance.displayTree();
    }

    private void initDetailPopUp()
    {
        detailPopUp = Instantiate(detailPopUpPrefab);
        detailPopUp.gameObject.SetActive(false);

        abilityDetailPopUp = Instantiate(abilityPopUpPrefab);
        abilityDetailPopUp.gameObject.SetActive(false);
    }

    public void displayLevelUp(int level)
    {
        levelUpDisplayer.displayLevelUp(level);
    }

    /// <summary>
    /// Display the description pop up
    /// </summary>
    /// <param name="display">Display the element or not</param>
    /// <param name="name">The name to display</param>
    /// <param name="description">The description to display</param>
    /// <param name="objectToDescribe">To object to describe (used to position the pop up)</param>
    public void displayDescription(bool display, IDescribable describable, MonoBehaviour objectToDescribe, bool smallSize = false)
    {
        if (detailPopUp == null || abilityDetailPopUp == null)
            initDetailPopUp();

        if(describable is Ability)
            displayAbilityDescription(display, (Ability)describable, objectToDescribe, smallSize);
        else
            displayBasicDescription(display, describable, objectToDescribe, smallSize);

    }

    void displayBasicDescription(bool display, IDescribable describable, MonoBehaviour objectToDescribe, bool smallSize )
    {
        if (display)
        {
            Canvas canvas = objectToDescribe.GetComponentInParent<Canvas>();
            if (canvas)
            {
                detailPopUp.transform.SetParent(canvas.transform);
                if (smallSize)
                    detailPopUp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                else
                    detailPopUp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Vector3 detailPopUpPosition = objectToDescribe.transform.position;

                detailPopUp.transform.position = detailPopUpPosition;
                detailPopUp.setText(describable, GameManager.instance.getPlayer());
            }
            else { return; }
        }
        detailPopUp.gameObject.SetActive(display);

    }

    void displayAbilityDescription(bool display, Ability ability, MonoBehaviour objectToDescribe, bool smallSize )
    {
        if (display)
        {
            Canvas canvas = objectToDescribe.GetComponentInParent<Canvas>();
            if (canvas)
            {
                abilityDetailPopUp.transform.SetParent(canvas.transform);
                if (smallSize)
                    abilityDetailPopUp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                else
                    abilityDetailPopUp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Vector3 detailPopUpPosition = objectToDescribe.transform.position;

                abilityDetailPopUp.transform.position = detailPopUpPosition;
                abilityDetailPopUp.setText(ability, GameManager.instance.getPlayer());
            }
            else { return; }
        }
        abilityDetailPopUp.gameObject.SetActive(display);

    }

}
