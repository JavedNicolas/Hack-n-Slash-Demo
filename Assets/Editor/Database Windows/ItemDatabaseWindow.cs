using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ItemDatabaseWindow : DatabaseWindows<ItemDatabaseModel>
{
    // database
    protected override string databasePath { get => "Databases/ItemDatabase"; }

    Vector2 formScrollPos;

    // item Field
    string itemName;
    Sprite itemIcon;
    GameObject itemModel;
    bool isConsomable = true;
    bool isStackable = true;
    bool canBeRecycle = true;
    int maxStackableSize = 10;
    ItemType itemType;
    ItemTargetType targetType;
    InteractableObjectType interactableType;
    int numberOfEffect;
    List<int> effectsIndex = new List<int>();
    List<ItemEffectAndValue> effects = new List<ItemEffectAndValue>();

    #region Right panel (The form)
    protected override void displayForm()
    {
        formScrollPos = EditorGUILayout.BeginScrollView(formScrollPos);
        EditorGUILayout.BeginVertical();
        GUI.enabled = false;
        databaseID = EditorGUILayout.IntField("Database ID : ", databaseID);
        GUI.enabled = true;
        itemName = EditorGUILayout.TextField("Name : ", itemName);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon : ", itemIcon, typeof(Sprite), false);
        itemModel = (GameObject)EditorGUILayout.ObjectField("Model : ", itemModel, typeof(GameObject), false);
        isConsomable = EditorGUILayout.Toggle("Is Consomable : ", isConsomable);
        isStackable = EditorGUILayout.Toggle("Is Stackable : ", isStackable);
        if (isStackable)
            maxStackableSize = EditorGUILayout.IntField("Max stack size : ", maxStackableSize);
        canBeRecycle = EditorGUILayout.Toggle("Can be recycle : ", canBeRecycle);
        interactableType = (InteractableObjectType)EditorGUILayout.EnumPopup("Interactible type : ", interactableType);
        itemType = (ItemType)EditorGUILayout.EnumPopup("Item type : ", itemType);
        targetType = (ItemTargetType)EditorGUILayout.EnumPopup("Target type : ", targetType);
        numberOfEffect = EditorGUILayout.IntField("Number Of effect :", numberOfEffect);
        updateEffectsListsSize(numberOfEffect);
        if(numberOfEffect != 0)
        {
            for (int i = 0; i < numberOfEffect; i++)
            {
                EditorGUILayout.LabelField("Effect n° " + i + " : ", centerTitle);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Effect :");
                effectsIndex[i] = EditorGUILayout.Popup(effectsIndex[i], EffectList.getEffectsNames());
                effects[i].effect = EffectList.effects[effectsIndex[i]];
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Value :");
                effects[i].value = EditorGUILayout.FloatField(effects[i].value);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Starting time :");
                effects[i].usedBy = (EffectUseBy)EditorGUILayout.EnumPopup(effects[i].usedBy);
                EditorGUILayout.EndHorizontal();
            }

        }

        displayFormButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    void updateEffectsListsSize(int size)
    {
        effects.updateSize(size);
        effectsIndex.updateSize(size);
    }
    #endregion

    protected override void setFieldWithElementValues()
    {
        if (element != null)
        {
            Item item = element.databaseModelToItem(resourcesList);
            databaseID = item.databaseID;
            itemName = item.name;
            itemIcon = item.itemIcon;
            itemModel = item.itemModel;
            isConsomable = item.isConsomable;
            isStackable = item.isStackable;
            canBeRecycle = item.canBeRecycle;
            maxStackableSize = item.maxStackableSize;
            interactableType = item.interactibleType;
            itemType = item.itemType;
            targetType = item.targetType;
            numberOfEffect = item.effects.Count;
            effects = item.effects;
            effectsIndex.updateSize(effects.Count);
            for(int i = 0; i < effects.Count; i++)
            {
                effectsIndex[i] = EffectList.getIndexfor(effects[i].effect.getName());
            }
        }
    }

    protected override void updateElementWithFormValues()
    {
        Item item = new Item(itemName, itemIcon, itemModel, isConsomable, isStackable, maxStackableSize, canBeRecycle, itemType,targetType, effects);
        item.databaseID = databaseID;
        item.interactibleType = interactableType;
        element = new ItemDatabaseModel(item, resourcesList);
    }

    protected override void clearForm()
    {
        base.clearForm();
        element = null;
        databaseID = database.getFreeId();
        itemName = "";
        itemIcon = null;
        itemModel = null;
        isConsomable = true;
        isStackable = true;
        canBeRecycle = false;
        maxStackableSize = 10;
        numberOfEffect = 0;
        interactableType = default;
        targetType = default;
        effects = new List<ItemEffectAndValue>();
    }
}