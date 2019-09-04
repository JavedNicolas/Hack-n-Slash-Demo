using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // instance
    public static GameUI instance;

    [Header("Base Screen UI")]
    [SerializeField] GameObject lifeUI;
    [SerializeField] GameObject manaUI;

    [Header("Openable UI")]
    [SerializeField] UIInventory inventoryUI;

    [Header("Description")]
    [SerializeField] Canvas detailPopUPCanvas;
    [SerializeField] UIDescriptionPopUp detailPopUpPrefab;
    [SerializeField] Vector3 detailPopUpOffset;

    UIDescriptionPopUp detailPopUp;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        inventoryUI.loadInventory();
        GameManager.instance.getPlayer().inventory.inventoryChanged();
        initDetailPopUp();
    }

    private void initDetailPopUp()
    {
        detailPopUp = Instantiate(detailPopUpPrefab);
        detailPopUp.gameObject.SetActive(false);
    }

    private void Update()
    {
        showInventory();
    }


    public void displayPlayerUI(Being player)
    {
        lifeUI.GetComponent<UILife>().setBeing(player);
        manaUI.GetComponent<UIMana>().setBeing((Player)player);
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


    /// <summary>
    /// Update inventory UI
    /// </summary>
    /// <param name="inventory"></param>
    public void updateInventoryUI(Inventory inventory)
    {
        inventoryUI.updateInventoryUI(inventory);
    }


    /// <summary>
    /// update a slot in the player inventory
    /// </summary>
    /// <param name="slots"></param>
    public void updateInventorySlots(List<InventorySlot> slots)
    {
        GameManager.instance.getPlayer().inventory.updateSlots(slots);
    }

    void showInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.showInventory(false);
        }
        if (Input.GetButtonDown("SmallInventory"))
        {
            inventoryUI.showInventory(true);
        }
    }

}
