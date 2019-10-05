using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class LootTests
    {
        Loot loot;

        #region SetUp
        [SetUp]
        public void setLoot()
        {
            loot = new Loot(null, 100, 1);
        }

        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.itemDatabase = Resources.Load<ItemDatabase>(DatabaseConstant.itemDatabasePath);
            GameManager.instance.enemyDatabase = Resources.Load<EnemyDatabase>(DatabaseConstant.databaseFolder + DatabaseConstant.enemyDatabaseFileName);
            GameManager.instance.resourcesList = Resources.Load<ResourcesList>(ScriptableObjectConstant.resourceListPath);
            GameManager.instance.loadDatabases();
        }
        #endregion

        #region Tests
        [Test]
        public void WhenInitializingLootWithALootWithNoItem_ThenGetARandomItem()
        {
            // assign 
            Loot newloot;

            // act
            newloot = new Loot(loot);

            // assert
            Assert.IsNotNull(newloot.item);
        }

        [Test]
        public void WhenCreatingANewEmptyLoot_ThenWeGetARandomLoot()
        {
            // assign 
            Loot newloot;

            // act
            newloot = new Loot();

            // assert
            Assert.IsNotNull(newloot.item);
        }

        [Test]
        public void WhenCreatingANewEmptyLootWithChangeToDropAndQuantity_ThenWeGetARandomLootWithSetQuantityAndChanceToDrop()
        {
            // assign 
            Loot newloot;

            // act
            newloot = new Loot(50, 10);

            // assert
            Assert.IsNotNull(newloot.item);
            Assert.AreEqual(50, newloot.chanceToDrop);
            Assert.AreEqual(10, newloot.quantity);
        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetGameManager()
        {
            GameManager.instance = null;

        }

        [TearDown]
        public void resetLoot()
        {
            loot = null;
        }
        #endregion
    }
}
