using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ItemTests
    {
        Item item;
        DatabaseResourcesList resourcesList;
        #region SetUp
        [SetUp]
        public void setItem()
        {
            item = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Potion, ItemTargetType.None, new List<ItemEffectAndValue>());
        }

        [SetUp]
        public void setResourceList()
        {
            resourcesList = (DatabaseResourcesList)ScriptableObject.CreateInstance(typeof(DatabaseResourcesList));
        }
        #endregion

        #region Tests
        [Test]
        public void ConvertItemToDatabaseModel()
        {
            // assign

            // act
            ItemDatabaseModel itemDatabaseModel = new ItemDatabaseModel(item, resourcesList);

            // assert
            Assert.IsNotNull(itemDatabaseModel);
            Assert.AreEqual(item.databaseID, itemDatabaseModel.databaseID);
            Assert.AreEqual(item.name, itemDatabaseModel.name);
            Assert.AreEqual(item.effects.Count, itemDatabaseModel.effectAndValues.Count);
        }

        [Test]
        public void ConvertDatabaseModelToItem()
        {
            // assign
            ItemDatabaseModel itemDatabaseModel = new ItemDatabaseModel(item, resourcesList);

            // act
            Item newItem = itemDatabaseModel.databaseModelToItem(resourcesList);

            // assert
            Assert.IsNotNull(newItem);
            Assert.AreEqual(item.databaseID, newItem.databaseID);
            Assert.AreEqual(item.name, newItem.name);
            Assert.AreEqual(item.effects.Count, newItem.effects.Count);

        }

        [Test]
        public void GivenWeHaveAnItemWithNoTarget_WhenWeTryToUseItOnNoTarget_ThemWeCanUseIt()
        {
            // assign

            // act
            bool canBeUsed = item.isCorrectTarget(null);

            // assert
            Assert.IsTrue(canBeUsed);
        }

        [Test]
        public void GivenWeHaveAnItemWithNoTarget_WhenWeTryToUseItOnATarget_ThemWeCantUseIt()
        {
            // assign

            // act
            bool canBeUsed = item.isCorrectTarget(new GameObject());

            // assert
            Assert.IsTrue(!canBeUsed);
        }

        [Test]
        public void GivenWeHaveAnItemWithAnyItemTarget_WhenWeTryToUseItOnAnItem_ThemWeCanUseIt()
        {
            // assign
            Item itemToUse = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Potion, ItemTargetType.AnyItem, new List<ItemEffectAndValue>());
            GameObject itemGameObject = new GameObject();
            itemGameObject.AddComponent<ItemObject>().setLoot(new Loot(item, 100, 1), false);

            // act
            bool canBeUsed = itemToUse.isCorrectTarget(itemGameObject);

            // assert
            Assert.IsTrue(canBeUsed);
        }

        [Test]
        public void GivenWeHaveAnItemWithAnyItemTarget_WhenWeTryToUseItOnNoItem_ThemWeCantUseIt()
        {
            // assign
            Item itemToUse = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Potion, ItemTargetType.AnyItem, new List<ItemEffectAndValue>());

            // act
            bool canBeUsed = itemToUse.isCorrectTarget(null);

            // assert
            Assert.IsTrue(!canBeUsed);
        }

        [Test]
        public void GivenWeHaveAnItemWithEquipementAsTarget_WhenWeTryToUseItOnAnEquipement_ThemWeCanUseIt()
        {
            // assign
            item = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Equipment, ItemTargetType.None, new List<ItemEffectAndValue>());
            Item itemToUse = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Potion, ItemTargetType.Equipment, new List<ItemEffectAndValue>());
            GameObject itemGameObject = new GameObject();
            itemGameObject.AddComponent<ItemObject>().setLoot(new Loot(item, 100, 1), false);

            // act
            bool canBeUsed = itemToUse.isCorrectTarget(itemGameObject);

            // assert
            Assert.IsTrue(canBeUsed);
        }

        [Test]
        public void GivenWeHaveAnItemWithEquipementAsTarget_WhenWeTryToUseItOnANonEquipementItem_ThemWeCantUseIt()
        {
            // assign
            Item itemToUse = new Item("potion", null, new GameObject(), true, true, 10, true, ItemType.Potion, ItemTargetType.Equipment, new List<ItemEffectAndValue>());
            GameObject itemGameObject = new GameObject();
            itemGameObject.AddComponent<ItemObject>().setLoot(new Loot(item, 100, 1), false);

            // act
            bool canBeUsed = itemToUse.isCorrectTarget(itemGameObject);

            // assert
            Assert.IsTrue(!canBeUsed);
        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetItem()
        {
            item = null;
        }
        #endregion
    }
}
