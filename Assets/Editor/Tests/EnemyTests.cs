using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EnemyTests
    {
        Enemy enemy;

        #region SetUp
        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.itemDatabase = Resources.Load<ItemDatabase>(DatabaseConstant.itemDatabasePath);
            GameManager.instance.enemyDatabase = Resources.Load<EnemyDatabase>(DatabaseConstant.databaseFolder + DatabaseConstant.enemyDatabaseFileName);
            GameManager.instance.resourcesList = Resources.Load<DatabaseResourcesList>(ScriptableObjectConstant.resourceListPath);
        }

        [SetUp]
        public void setPlayer()
        {
            enemy = new Enemy("Enemy", 100, 1, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 20f) ;
        }
        #endregion

        #region Tests
        [Test]
        public void WhenLoadingEnemyFromDatabase_ThenEnemyIsInitializedWithTheDatabaseValues()
        {
            // assign 
            Enemy dbEnemy = GameManager.instance.enemyDatabase.getRandomElement().databaseModelToEnemy(GameManager.instance.resourcesList);

            // act
            enemy = new Enemy(dbEnemy);

            // Assert
            Assert.AreEqual(dbEnemy.getName(), enemy.getName());
            Assert.AreEqual(dbEnemy.databaseID, enemy.databaseID);
        }

        [Test]
        public void WhenGeneratingLoot_TheyComeFromPossibleLootList()
        {
            //assign 
            enemy = GameManager.instance.enemyDatabase.getRandomElement().databaseModelToEnemy(GameManager.instance.resourcesList);

            //act
            List<Loot> loots = enemy.generateLoot();

            //assert
            for(int i = 0; i < loots.Count; i++)
            {
                Assert.IsTrue(enemy.possibleLoot.Contains(loots[i]));
            }
            
        }

        // Testing enemy database Model
        [Test]
        public void WhenWeNeedToSaveEnemyInDatabase_ThenConvertItToEnemyDatabaseModel()
        {
            // assign
            enemy = GameManager.instance.enemyDatabase.getRandomElement().databaseModelToEnemy(GameManager.instance.resourcesList);

            // act
            EnemyDatabaseModel enemyDatabaseModel = new EnemyDatabaseModel(enemy, "GUID");

            // assert
            Assert.IsNotNull(enemyDatabaseModel);
            Assert.AreEqual(enemy.databaseID, enemyDatabaseModel.databaseID);
        }

        [Test]
        public void GiveWeNeedAnEnemyFromTheDatabase_WhenWeGetTheDatabaseModel_ThenWeCanConvertItToAndEnemy()
        {
            // assign
            EnemyDatabaseModel enemyDatabaseModel = GameManager.instance.enemyDatabase.getRandomElement();

            // act
            Enemy enemy = enemyDatabaseModel.databaseModelToEnemy(GameManager.instance.resourcesList);

            // Assert
            Assert.IsNotNull(enemy);
            Assert.AreEqual(enemyDatabaseModel.databaseID, enemy.databaseID);
        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetGameManager()
        {
            GameManager.instance = null;
        }
        #endregion

    }
}
