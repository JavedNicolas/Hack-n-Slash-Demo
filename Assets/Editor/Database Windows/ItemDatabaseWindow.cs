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
    TargetType targetType;
    InteractableObjectType interactableType;
    int numberOfEffect;
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
        targetType = (TargetType)EditorGUILayout.EnumPopup("Target type : ", targetType);
        numberOfEffect = EditorGUILayout.IntField("Number Of effect :", numberOfEffect);
        updateEffectsListsSize(numberOfEffect);
        if(numberOfEffect != 0)
        {
            for (int i = 0; i < numberOfEffect; i++)
            {
                EditorGUILayout.LabelField("Effect n° " + i + " : ", centerTitle);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Effect :");
                effects[i].effect = (Effect)EditorGUILayout.ObjectField(effects[i].effect, typeof(Effect), true);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Value :");
                effects[i].value = EditorGUILayout.FloatField(effects[i].value);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Starting time :");
                effects[i].startingTime = (EffectType)EditorGUILayout.EnumPopup(effects[i].startingTime);
                EditorGUILayout.EndHorizontal();
            }

        }

        displayFormButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    void updateEffectsListsSize(int size)
    {
        while (effects.Count != size)
        {
            if (effects.Count < size)
            {
                effects.Add(new ItemEffectAndValue());
            }
            else if (effects.Count > size)
            {
                effects.RemoveAt(effects.Count - 1);
            }
        }

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
            targetType = item.targetType;
            numberOfEffect = item.effects.Count;
            effects = item.effects;
        }
    }

    protected override void updateElementWithFormValues()
    {
        string spriteGUID = resourcesList.addObjects(itemIcon);
        string modelGUID = resourcesList.addObjects(itemModel);
        List<ItemEffectAndValuesDatabaseModel> itemEffectAndValuesDatabaseModels = new List<ItemEffectAndValuesDatabaseModel>();
        foreach(ItemEffectAndValue itemEffectAndValue in effects)
        {
            string effectGUID = resourcesList.addObjects(itemEffectAndValue.effect);
            itemEffectAndValuesDatabaseModels.Add(new ItemEffectAndValuesDatabaseModel(itemEffectAndValue.value, effectGUID, itemEffectAndValue.startingTime, itemEffectAndValue.statTypes));
        }
        element = new ItemDatabaseModel(databaseID, itemName, spriteGUID, modelGUID, isConsomable, isStackable, canBeRecycle, maxStackableSize, targetType, interactableType, itemEffectAndValuesDatabaseModels);
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