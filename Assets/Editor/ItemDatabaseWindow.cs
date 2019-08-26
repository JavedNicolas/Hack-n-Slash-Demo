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
        itemName = EditorGUILayout.TextField("Name : ", itemName);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon : ", itemIcon, typeof(Sprite), false);
        itemModel = (GameObject)EditorGUILayout.ObjectField("Model : ", itemModel, typeof(GameObject), false);
        isConsomable = EditorGUILayout.Toggle("Is Consomable : ", isConsomable);
        isStackable = EditorGUILayout.Toggle("Is Stackable : ", isStackable);
        maxStackableSize = EditorGUILayout.IntField("Max stack size : ", maxStackableSize);
        interactableType = (InteractableObjectType)EditorGUILayout.EnumPopup("Interactible type : ", interactableType);
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
            itemName = element.name;
            itemIcon = element.itemIcon;
            itemModel = element.itemModel;
            isConsomable = element.isConsomable;
            isStackable = element.isStackable;
            maxStackableSize = element.maxStackableSize;
            numberOfEffect = element.effects.effects.Count;
            effects = element.effects;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Item(itemName, itemIcon, itemModel, isConsomable, isStackable, maxStackableSize, effects);
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
        numberOfEffect = 0;
        effects = new EffectAndValues();
    }

    protected override string getNameAtIndex(int index)
    {
        DatabaseElement item = database.getElementAt(index);
        if (item != null)
            return item.name;

        return "";
    }
}