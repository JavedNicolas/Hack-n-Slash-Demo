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
    [SerializeField] public InteractableObjectType interactableType;
    [SerializeField] public List<ItemEffectAndValuesDatabaseModel> effectAndValues = new List<ItemEffectAndValuesDatabaseModel>();

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
        this.interactableType = interactableType;
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
        item.interactibleType = interactableType;
        return item;
    }
}
