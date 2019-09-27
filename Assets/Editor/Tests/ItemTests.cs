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
            item = new Item("potion", null, new GameObject(), true, true, 10, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
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
