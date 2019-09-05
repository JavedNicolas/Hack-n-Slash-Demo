using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    [Header("Description")]
    [SerializeField] public Canvas detailPopUPCanvas;
    [SerializeField] public UIDescriptionPopUp detailPopUpPrefab;
    [SerializeField] public Vector3 detailPopUpOffset;

    UIDescriptionPopUp detailPopUp;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        initDetailPopUp();
    }

    private void initDetailPopUp()
    {
        detailPopUp = Instantiate(detailPopUpPrefab);
        detailPopUp.gameObject.SetActive(false);
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
        if (detailPopUp == null)
            initDetailPopUp();

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
                detailPopUp.setText(describable);
            }
            else { return; }
        }
        detailPopUp.gameObject.SetActive(display);

    }
}
