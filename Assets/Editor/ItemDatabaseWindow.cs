using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemDatabaseWindow : DatabaseWindows<Item>
{
    // database
    protected override string databasePath { get => "Databases/ItemDatabase"; }

    // item Field
    string itemName;
    int databaseID;
    Sprite itemIcon;
    GameObject itemModel;
    bool isConsomable = true;
    bool isStackable = true;
    int maxStackableSize = 10;
    InteractableObjectType interactableType;

    [MenuItem("Database/Item")]
    public static void databadeMenuFunction()
    {
        windows = new ItemDatabaseWindow();
        windows.showWindows();
    }

    #region Right panel (The form)
    protected override void displayForm()
    {
        EditorGUILayout.BeginVertical();
        itemName = EditorGUILayout.TextField("Name : ", itemName);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon : ", itemIcon, typeof(Sprite), false);
        itemModel = (GameObject)EditorGUILayout.ObjectField("Model : ", itemModel, typeof(GameObject), false);
        isConsomable = EditorGUILayout.Toggle("Is Consomable : ", isConsomable);
        isStackable = EditorGUILayout.Toggle("Is Stackable : ", isStackable);
        maxStackableSize = EditorGUILayout.IntField("Max stack size : ", maxStackableSize);
        interactableType = (InteractableObjectType)EditorGUILayout.EnumPopup("Interactible type : ", interactableType);

        displayFormButtons();
        EditorGUILayout.EndVertical();
    }
    #endregion


    protected override void setFieldWithElementValues()
    {
        if (element != null)
        {
            itemName = element.name;
            itemIcon = element.itemIcon;
            itemModel = element.itemModel;
            isConsomable = element.isConsomable;
            isStackable = element.isStackable;
            maxStackableSize = element.maxStackableSize;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Item(itemName, itemIcon, itemModel, isConsomable, isStackable, maxStackableSize);
        element.interactibleType = interactableType;
    }

    protected override void clearForm()
    {
        base.clearForm();
        element = new Item();
        itemName = "";
        itemIcon = null;
        itemModel = null;
        isConsomable = false;
        isStackable = false;
        maxStackableSize = 0;
    }

    protected override string getNameAtIndex(int index)
    {
        DatabaseElement item = database.getElementAt(index);
        if (item != null)
            return item.name;

        return "";
    }
}