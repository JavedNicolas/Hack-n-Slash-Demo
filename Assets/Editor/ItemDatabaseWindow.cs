using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ItemDatabaseWindow : DatabaseWindows<Item>
{
    // database
    protected override string databasePath { get => "Databases/ItemDatabase"; }

    // item Field
    string itemName;
    Sprite itemIcon;
    GameObject itemModel;
    bool isConsomable = true;
    bool isStackable = true;
    int maxStackableSize = 10;
    TargetType targetType;
    InteractableObjectType interactableType;
    int numberOfEffect;
    EffectAndValues effects = new EffectAndValues();

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
        GUI.enabled = false;
        databaseID = EditorGUILayout.IntField("Database ID : ", databaseID);
        GUI.enabled = true;
        itemName = EditorGUILayout.TextField("Name : ", itemName);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon : ", itemIcon, typeof(Sprite), false);
        itemModel = (GameObject)EditorGUILayout.ObjectField("Model : ", itemModel, typeof(GameObject), false);
        isConsomable = EditorGUILayout.Toggle("Is Consomable : ", isConsomable);
        isStackable = EditorGUILayout.Toggle("Is Stackable : ", isStackable);
        maxStackableSize = EditorGUILayout.IntField("Max stack size : ", maxStackableSize);
        interactableType = (InteractableObjectType)EditorGUILayout.EnumPopup("Interactible type : ", interactableType);
        targetType = (TargetType)EditorGUILayout.EnumPopup("Target type : ", targetType);
        numberOfEffect = EditorGUILayout.IntField("Number Of effect :", numberOfEffect);
        updateEffectsListsSize(numberOfEffect);
        if(numberOfEffect != 0)
        {
            EditorGUILayout.LabelField("Effects : ");
            for (int i = 0; i < numberOfEffect; i++)
            {
                effects.effects[i] = (Effect)EditorGUILayout.ObjectField("Effect n°" + i + " :", effects.effects[i], typeof(Effect), true);
            }

            EditorGUILayout.LabelField("Values");
            for (int i = 0; i < numberOfEffect; i++)
            {
                effects.effectValues[i] = EditorGUILayout.FloatField("Value for Effect n°" + i + " :", effects.effectValues[i]);
            }
        }
        


        displayFormButtons();
        EditorGUILayout.EndVertical();
    }
    #endregion

    void updateEffectsListsSize(int size)
    {
        while(effects.effects.Count != size && effects.effectValues.Count != size)
        {
            if(effects.effects.Count < size)
            {
                effects.effects.Add(null);
            }else if(effects.effects.Count > size)
            {
                effects.effects.RemoveAt(effects.effects.Count - 1);
            }
            if (effects.effectValues.Count < size)
            {
                effects.effectValues.Add(0);
            }
            else if (effects.effectValues.Count > size)
            {
                effects.effectValues.RemoveAt(effects.effectValues.Count - 1);
            }
        }

    }

    protected override void setFieldWithElementValues()
    {
        if (element != null)
        {
            databaseID = element.databaseID;
            itemName = element.name;
            itemIcon = element.itemIcon;
            itemModel = element.itemModel;
            isConsomable = element.isConsomable;
            isStackable = element.isStackable;
            maxStackableSize = element.maxStackableSize;
            interactableType = element.interactibleType;
            targetType = element.targetType;
            numberOfEffect = element.effects.effects.Count;
            effects = element.effects;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Item(itemName, itemIcon, itemModel, isConsomable, isStackable, maxStackableSize, targetType,effects);
        element.interactibleType = interactableType;
        element.databaseID = databaseID;
    }

    protected override void clearForm()
    {
        base.clearForm();
        element = new Item();
        databaseID = database.getFreeId();
        itemName = "";
        itemIcon = null;
        itemModel = null;
        isConsomable = true;
        isStackable = true;
        maxStackableSize = 10;
        numberOfEffect = 0;
        interactableType = default;
        targetType = default;
        effects = new EffectAndValues();
    }
}