using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTests
    {
        Player player;

        #region SetUp
        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.resourcesList = Resources.Load<DatabaseResourcesList>(ScriptableObjectConstant.resourceListPath);
        }

        [SetUp]
        public void setPlayer()
        { 
            player = new Player("Player", 100, 20, 1, 1, 10, new List<int>(), null, 1);
        }
        #endregion

        #region Tests
        [Test]
        public void WhenPlayerSpendMana_ThenHisManaIsReducedByTheManaCost()
        {
            // assign
            float manaCost = 10f;
            float baseMana = player.currentMana;

            // act
            player.spendMana(manaCost);

            // assert
            Assert.AreEqual(baseMana - manaCost, player.currentMana);
        }

        [Test]
        public void WhenAddingExperienceToThePlayer_ThenHisAbilitiesHaveGainedExperience()
        {
            // assign
            float expToAdd = 10f;
            player.addAbility(0);

            // act
            player.addAllExperience(expToAdd);

            // assert
            Assert.AreEqual(expToAdd, player.abilities[0].stats.currentLevelExp);

        }

        [Test]
        public void WhenAddingEnoughtExperienceToLevelUPThePlayer_ThenTheCurrentPlayerLevelIncreased()
        {
            // assign
            float exp = LevelExperienceTable.levelExperienceNeeded[player.stats.currentLevel];

            // act
            player.addAllExperience(exp);

            // assert
            Assert.AreEqual(2, player.stats.currentLevel);

        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetGameManager()
        {
            GameManager.instance = null;
        }

        [TearDown]
        public void resetPlayer()
        {
            player = null;
        }
        #endregion

    }
}
