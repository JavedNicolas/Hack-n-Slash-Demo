using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemDatabaseModel : DatabaseElement
{
    [SerializeField] public string spriteGUID;
    [SerializeField] public string modelGUID;
    [SerializeField] public bool isConsomable = true;
    [SerializeField] public bool isStackable = true;
    [SerializeField] public bool canBeRecycle = false;
    [SerializeField] public int maxStackableSize = 10;
    [SerializeField] public TargetType targetType;
    [SerializeField] public InteractableObjectType interactibleType;
    [SerializeField] public List<ItemEffectAndValuesDatabaseModel> effectAndValues = new List<ItemEffectAndValuesDatabaseModel>();

    public ItemDatabaseModel(Item item, string spriteGUID, string modelGUID, List<ItemEffectAndValuesDatabaseModel> itemEffectAndValues)
    {
        this.databaseID = item.databaseID;
        this.name = item.name;
        this.spriteGUID = spriteGUID;
        this.modelGUID = modelGUID;
        this.isConsomable = item.isConsomable;
        this.isStackable = item.isStackable;
        this.canBeRecycle = item.canBeRecycle;
        this.maxStackableSize = item.maxStackableSize;
        this.targetType = item.targetType;
        this.interactibleType = item.interactibleType;
        this.effectAndValues = itemEffectAndValues;
    }


    public ItemDatabaseModel(int databaseID, string name, string spriteGUID, string modelGUID, bool isConsomable, bool isStackable, bool canBeRecycle, int maxStackableSize, TargetType targetType,
        InteractableObjectType interactableType, List<ItemEffectAndValuesDatabaseModel> itemEffectAndValues)
    {
        this.databaseID = databaseID;
        this.name = name;
        this.spriteGUID = spriteGUID;
        this.modelGUID = modelGUID;
        this.isConsomable = isConsomable;
        this.isStackable = isStackable;
        this.canBeRecycle = canBeRecycle;
        this.maxStackableSize = maxStackableSize;
        this.targetType = targetType;
        this.interactibleType = interactableType;
        this.effectAndValues = itemEffectAndValues;
    }

    public Item databaseModelToItem(DatabaseResourcesList resourcesList)
    {
        Sprite sprite = (Sprite)resourcesList.getObject<Sprite>(spriteGUID);
        GameObject model = (GameObject)resourcesList.getObject<GameObject>(modelGUID);

        List<ItemEffectAndValue> itemEffectAndValues = new List<ItemEffectAndValue>();
        if(effectAndValues != null)
            foreach(ItemEffectAndValuesDatabaseModel databaseModel in effectAndValues)
            {
                itemEffectAndValues.Add(databaseModel.dataBaseModelToItemEffectAndValue(resourcesList));
            }
        Item item = new Item(name, sprite, model, isConsomable, isStackable, maxStackableSize, canBeRecycle, targetType, itemEffectAndValues);
        item.databaseID = databaseID;
        item.interactibleType = interactibleType;
        return item;
    }
}
