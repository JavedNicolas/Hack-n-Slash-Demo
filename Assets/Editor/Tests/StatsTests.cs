using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StatsTests
    {
        PlayerStats playerStats;

        #region SetUp
        [SetUp]
        public void setStats()
        {
            playerStats = new PlayerStats(1);
        }

        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.resourcesList = Resources.Load<ResourcesList>(ScriptableObjectConstant.resourceListPath);
            GameManager.instance.loadDatabases();
        }
        #endregion

        #region Tests
        [Test]
        public void WhenWeWantToAddAStat_ThenThereIsAStatsInTheStatsList()
        {
            // assign
            Stat stat = new Stat(StatType.Life, StatBonusType.additional, 10, "");

            // act
            playerStats.addStat(stat);

            // assert
            Assert.Contains(stat, playerStats.statList);
        }

        [Test]
        public void WhenRemovingAStats_ThenTheStatListDoesNotHaveThoseStatAnymore()
        {
            // assign
            Stat stat = new Stat(StatType.Life, StatBonusType.Pure, 10, "test");
            Stat stat2 = new Stat(StatType.Mana, StatBonusType.Pure, 10, "test");

            // act
            playerStats.addStat(stat);
            playerStats.addStat(stat2);
            playerStats.removeStat("test");

            // asserts
            Assert.IsTrue(!playerStats.statList.Contains(stat));
            Assert.IsTrue(!playerStats.statList.Contains(stat2));
        }

        [Test]
        public void WhenRecevingExp_ThenIncreaseTheCurrentLevelExperience()
        {
            // assign
            float exp = 10;

            // act
            playerStats.addExperience(exp, LevelExperienceTable.levelExperienceNeeded);

            // assert
            Assert.AreEqual(exp, playerStats.currentLevelExp);
        }

        [Test]
        public void WhenRecevingEnoughExperienceToLevelUp_ThenCurrentLevelIsIncreasedAExperienceAndSpareExperienceIsReused()
        {
            //assign
            float exp = LevelExperienceTable.levelExperienceNeeded[1] + 20f;

            // act
            playerStats.addExperience(exp, LevelExperienceTable.levelExperienceNeeded);

            // assert
            Assert.AreEqual(2, playerStats.currentLevel);
            Assert.AreEqual(20f, playerStats.currentLevelExp);
        }

        [Test]
        public void GivenThereIsStat_WhenWeAskForTheFloatBuffedValue_ThenWeGetTheFloatValueModifiedByTheStats()
        {
            // assign
            Stat stat = new Stat(StatType.Life, StatBonusType.Pure, 10, "test");
            playerStats.addStat(stat);

            // act
            float buffedFloatValue = playerStats.getBuffedValue(100f, StatType.Life);

            // assert
            Assert.AreEqual(100f + 10f, buffedFloatValue);
        }

        [Test]
        public void GivenThereIsStat_WhenWeAskForTheIntBuffedValue_ThenWeGetTheIntValueModifiedByTheStats()
        {
            // assign
            Stat stat = new Stat(StatType.Life, StatBonusType.Pure, 10, "test");
            playerStats.addStat(stat);

            // act
            int buffedFloatValue = playerStats.getBuffedValue(100, StatType.Life);

            // assert
            Assert.AreEqual(100 + 10, buffedFloatValue);
        }

        [Test]
        public void GivenThereIsStatSpecificToAnAbility_WhenWeAskForTheBuffedValue_ThenWeGetTheValueModifiedByTheStatsAndThoseSpecificToTheAbility()
        {
            // assign
            Stat stat = new Stat(StatType.ProjectileDamage, StatBonusType.Pure, 10f, "test", "LightningBall");
            playerStats.addStat(stat);

            // act
            float buffedFloatValueWithAbilityStats = playerStats.getBuffedValue(100f, StatType.ProjectileDamage, "LightningBall");
            float buffedFloatValueWithoutAbilityStats = playerStats.getBuffedValue(100f, StatType.ProjectileDamage);

            // assert
            Assert.AreEqual(100f + 10f, buffedFloatValueWithAbilityStats);
            Assert.AreEqual(100f, buffedFloatValueWithoutAbilityStats);
        }

        [Test]
        public void WhenWeNeedTheListOfStatDescribingAGivenAbility_ThenWeRetrivedAListOfStringDecription()
        {
            // assign
            Stat stat = new Stat(StatType.ProjectileDamage, StatBonusType.Pure, 10f, "test", "LightningBall");
            playerStats.addStat(stat);
            Ability ability = GameManager.instance.abilityDatabase.getAbilityFromDatabaseID(0, GameManager.instance.resourcesList);
            int abilityStatsCount = ability.stats.statList.Count;

            // act
            List<string> statforLightningBall = playerStats.getBonusListFor(ability);

            // Assert
            Assert.AreEqual(1 + abilityStatsCount, statforLightningBall.Count);
        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetStats()
        {
            playerStats = null;
        }

        [TearDown]
        public void resetTheGameManager()
        {
            GameManager.instance = null;
        }
        #endregion
    }
}
