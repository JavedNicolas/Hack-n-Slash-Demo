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
    [SerializeField] public ItemType itemType;
    [SerializeField] public ItemTargetType targetType;
    [SerializeField] public InteractableObjectType interactibleType;
    [SerializeField] public List<ItemEffectAndValuesDatabaseModel> effectAndValues = new List<ItemEffectAndValuesDatabaseModel>();

    public ItemDatabaseModel(Item item, DatabaseResourcesList resourcesList)
    {
        this.databaseID = item.databaseID;
        this.name = item.name;
        this.spriteGUID = resourcesList.getGUIDFor(item.itemIcon);
        this.modelGUID = resourcesList.getGUIDFor(item.itemModel);
        this.isConsomable = item.isConsomable;
        this.isStackable = item.isStackable;
        this.canBeRecycle = item.canBeRecycle;
        this.maxStackableSize = item.maxStackableSize;
        this.itemType = item.itemType;
        this.targetType = item.targetType;
        this.interactibleType = item.interactibleType;
        List<ItemEffectAndValuesDatabaseModel> itemEffectAndValues = new List<ItemEffectAndValuesDatabaseModel>();
        foreach(ItemEffectAndValue effectAndValue in item.effects)
        {
            itemEffectAndValues.Add(new ItemEffectAndValuesDatabaseModel(effectAndValue, resourcesList));
        }
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
        Item item = new Item(name, sprite, model, isConsomable, isStackable, maxStackableSize, canBeRecycle, itemType, targetType, itemEffectAndValues);
        item.databaseID = databaseID;
        item.interactibleType = interactibleType;
        return item;
    }
}
