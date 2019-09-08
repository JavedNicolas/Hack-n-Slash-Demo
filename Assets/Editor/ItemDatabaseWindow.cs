using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ItemDatabaseWindow : DatabaseWindows<Item>
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
    List<EffectAndValue> effects = new List<EffectAndValue>();

    [MenuItem("Database/Item")]
    public static void databadeMenuFunction()
    {
        windows = new ItemDatabaseWindow();
        windows.showWindows();
    }

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
                effects[i].value = EditorGUILayout.FloatField( effects[i].value);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Starting time :");
                effects[i].startingTime = (EffectStartingTime)EditorGUILayout.EnumPopup(effects[i].startingTime);
                EditorGUILayout.EndHorizontal();
            }

        }

        displayFormButtons();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
    #endregion

    void updateEffectsListsSize(int size)
    {
        while(effects.Count != size)
        {
            if(effects.Count < size)
            {
                effects.Add(new EffectAndValue());
            }else if(effects.Count > size)
            {
                effects.RemoveAt(effects.Count - 1);
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
            canBeRecycle = element.canBeRecycle;
            maxStackableSize = element.maxStackableSize;
            interactableType = element.interactibleType;
            targetType = element.targetType;
            numberOfEffect = element.effects.Count;
            effects = element.effects;
        }
    }

    protected override void updateElementWithFormValues()
    {
        element = new Item(itemName, itemIcon, itemModel, isConsomable, isStackable, maxStackableSize, canBeRecycle,targetType,effects);
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
        canBeRecycle = false;
        maxStackableSize = 10;
        numberOfEffect = 0;
        interactableType = default;
        targetType = default;
        effects = new List<EffectAndValue>();
    }
}