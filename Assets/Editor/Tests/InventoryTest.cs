using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InventoryTest
    {
        Inventory inventory;
        #region SetUp
        [SetUp]
        public void setInventory()
        {
            inventory = new Inventory();
        }
        #endregion
        [Test]
        public void WhenAddingSlotInTheInvetory_ThenTheNumberOfSlotIsIncreased()
        {
            // assign
            int numberOfSlotToAdd = 5;
            int baseNumberOfSlot = inventory.numberOfSlot;
            int numberOfSlotNeeded = baseNumberOfSlot + numberOfSlotToAdd;

            // act
            bool updateSucceed = inventory.updateInventorySize(numberOfSlotNeeded);

            // assert
            Assert.IsTrue(updateSucceed);
            Assert.AreEqual(numberOfSlotNeeded, inventory.numberOfSlot);
            Assert.AreEqual(numberOfSlotNeeded, inventory.slots.Count);
        }

        [Test]
        public void WhenRemovingSlotInTheInvetory_ThenTheNumberOfSlotIsDecreased()
        {
            // assign
            int numberOfSlotToRemove = 2;
            int baseNumberOfSlot = inventory.numberOfSlot;
            int numberOfSlotNeeded = baseNumberOfSlot - numberOfSlotToRemove;

            // act
            bool updateSucceed = inventory.updateInventorySize(numberOfSlotNeeded);

            // assert
            Assert.IsTrue(updateSucceed);
            Assert.AreEqual(numberOfSlotNeeded, inventory.numberOfSlot);
            Assert.AreEqual(numberOfSlotNeeded, inventory.slots.Count);
        }

        [Test]
        public void WhenRemovingSlotsInTheInventoryButItsGoindToGoBelowMinimumSlot_ThenWeRemoveOnlyUntilWeReachTheMinimumSlotCount()
        {
            //assign 
            int numberOfSlotNeeded = inventory.minimumSlotsCount - 1;

            // act
            bool updateSucceed = inventory.updateInventorySize(numberOfSlotNeeded);

            // assert
            Assert.IsTrue(updateSucceed);
            Assert.AreEqual(inventory.minimumSlotsCount, inventory.slots.Count);
            Assert.AreEqual(inventory.minimumSlotsCount, inventory.numberOfSlot);
        }

        [Test]
        public void WhenAddingSlotsInTheInventoryButItsGoindToGoBelowMaximumSlot_ThenWeAddOnlyUntilWeReachTheMaximumSlotCount()
        {
            //assign 
            int numberOfSlotNeeded = inventory.maximumSlotsCoun + 1;

            // act
            bool updateSucceed = inventory.updateInventorySize(numberOfSlotNeeded);

            // assert
            Assert.IsTrue(updateSucceed);
            Assert.AreEqual(inventory.maximumSlotsCoun, inventory.slots.Count);
            Assert.AreEqual(inventory.maximumSlotsCoun, inventory.numberOfSlot);
        }



        [Test]
        public void WhenRemovingSlotsInTheInventoryButThereIsNotEnoughtEmptySlot_ThenWeDontRemoveSlotAndReturnFalse()
        {
            //assign 
            Item item = new Item("potion", null, new GameObject(), true, true, 1, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;

            // reduce inventorySize by 2
            int numberOfSlotNeed = inventory.numberOfSlot - 2;

            // fill all slot except 1
            int baseNumberOfSlot = inventory.numberOfSlot;
            inventory.addToInventory(item, inventory.numberOfSlot - 1);

            // act
            bool updateSucceed = inventory.updateInventorySize(numberOfSlotNeed);

            // assert
            Assert.IsTrue(!updateSucceed);
            Assert.AreEqual(baseNumberOfSlot, inventory.numberOfSlot);

        }

        [Test]
        public void WhenAddingItemWithNoQuanityInTheInventory_ThenThereIsASlotContainingIt()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, true, 10, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;

            //act
            inventory.addToInventory(item);

            // assert
            InventorySlot slot = inventory.slots.Find(x => x.item?.databaseID == item.databaseID);
            Assert.AreEqual(item, slot.item);
        }

        [Test]
        public void WhenAddingItemWithQuanityBelowStackableLimitInTheInventory_ThenThereIsASlotContainingThoseItems()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, true, 10, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;

            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, 5);
            InventorySlot slot = inventory.slots.Find(x => x.item?.databaseID == item.databaseID);

            // assert
            Assert.IsTrue(itemAddedWithSuccess);
            Assert.AreEqual(item, slot.item);
            Assert.AreEqual(5, slot.quantity);
        }

        [Test]
        public void WhenAddingItemWithQuanityAboveStackableLimitInTheInventoryWithAvailableSlots_ThenThereIsASlotsContainingThoseItems()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, true, 10, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;

            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, 15);
            List<InventorySlot> slots = inventory.slots.FindAll(x => x.item?.databaseID == item.databaseID);
            int numberOfItemInInventory = 0;

            // assert
            Assert.IsTrue(itemAddedWithSuccess);
            foreach (InventorySlot slot in slots)
            {
                Assert.AreEqual(item, slot.item);
                numberOfItemInInventory += slot.quantity;
            }
            Assert.AreEqual(15, numberOfItemInInventory);
        }

        [Test]
        public void WhenAddingItemWithQuanityAboveStackableLimitInTheInventoryWithNotEnoughtAvailableSlots_ThenTheItemIsNotAdded()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, true, 1, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;
            inventory.updateInventorySize(inventory.minimumSlotsCount);

            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, inventory.minimumSlotsCount + 1);
            int numberOfItemInInventory = inventory.slots.FindAll(x => x.item?.databaseID == item.databaseID).Count;

            // assert
            Assert.IsTrue(!itemAddedWithSuccess);
            Assert.AreEqual(0, numberOfItemInInventory);
        }

        [Test]
        public void WhenAddingItemWithQuantityAndNotStackableWithEnoughtEmptySlot_ThenTheItemsAreAdded()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, false, 1, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;
            int numberOfItemToAdd = inventory.minimumSlotsCount - 1;

            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, numberOfItemToAdd);
            int numberOfItemInInventory = inventory.slots.FindAll(x => x.item?.databaseID == item.databaseID).Count;

            // assert
            Assert.IsTrue(itemAddedWithSuccess);
            Assert.AreEqual(numberOfItemToAdd, numberOfItemInInventory);
        }

        [Test]
        public void WhenAddingItemWithQuantityAndNotStackableWithNotEnoughtEmptySlot_ThenTheItemsAreNotAdded()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, false, 1, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;
            int numberOfItemToAdd = inventory.numberOfSlot + 1;

            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, numberOfItemToAdd);
            int numberOfItemInInventory = inventory.slots.FindAll(x => x.item?.databaseID == item.databaseID).Count;

            // assert
            Assert.IsTrue(!itemAddedWithSuccess);
            Assert.AreEqual(0, numberOfItemInInventory);
        }


        [Test]
        public void WhenAddingItemWithQuanityAboveStackableLimitInTheInventoryWithNoAvailableSlots_ThenTheItemIsNotAdded()
        {
            // assign
            Item item = new Item("potion", null, new GameObject(), true, true, 1, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
            item.databaseID = 1;
            inventory.updateInventorySize(inventory.minimumSlotsCount);
            // fill the inventory with this item
            Item fillingItem = new Item("filling Item", null, new GameObject(), true, true, 1, true, TargetType.Equipment, new List<ItemEffectAndValue>());
            fillingItem.databaseID = 2;
            inventory.addToInventory(fillingItem, inventory.minimumSlotsCount);
           
            //act
            bool itemAddedWithSuccess = inventory.addToInventory(item, 10);
            int numberOfItemInInventory = inventory.slots.FindAll(x => x.item?.databaseID == item.databaseID).Count;

            // assert
            Assert.IsTrue(!itemAddedWithSuccess);
            Assert.AreEqual(0, numberOfItemInInventory);
        }

        #region Tests

        #endregion

        #region TearDowns
        [TearDown]
        public void resetInventory()
        {
            inventory = null;
        }
        #endregion
    }
}
