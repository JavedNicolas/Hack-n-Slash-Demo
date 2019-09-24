using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BeingTests
    {
        Being being;
        #region SetUp
        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.resourcesList = Resources.Load<DatabaseResourcesList>(ScriptableObjectConstant.resourceListPath);
            
        }

        [SetUp]
        public void setBeing()
        {
            being = new Being("test", 100, 1, 1, 10, 10, new List<int>(), null);
        }
        #endregion

        #region tests
        // A Test behaves as an ordinary method
        [Test]
        public void WhenCreatingABeingWithAbilityIDS_ThenTheBeingHaveAbilities()
        {
            //Assign

            //Act
            being.addAbility(0, true);
            being.addAbility(1, true);

            // Assert
            Assert.AreEqual(2, being.abilities.Count);
        }

        [Test]
        public void WhenAddingALifeStatToABeing_ThenTheBaseLifeIsChanged()
        {
            //Assign
            float baseLife = being.stats.maxLife;

            // Act and assert
            being.stats.addStat(new Stat(StatType.Life, StatBonusType.Pure, 10, ""));

            Assert.AreEqual(baseLife + 10, being.stats.maxLife);

            being.stats.addStat(new Stat(StatType.Life, StatBonusType.additional, 100, ""));

            Assert.AreEqual(baseLife + 120, being.stats.maxLife);

            being.stats.addStat(new Stat(StatType.Life, StatBonusType.Multiplied, 100, ""));

            Assert.AreEqual(baseLife + 340, being.stats.maxLife);
        }

        [Test]
        public void WhenAddingAStatInfluencedBySomething_ThenTargetOfTheStatIsInfluenced()
        {
            //Assign
            float baseLife = being.stats.maxLife;

            // Act
            being.stats.addStat(new Stat(StatType.Life, StatBonusType.Pure, 1, "", StatInfluencedBy.Strength, 10));
            // add intelligence to modify life
            being.stats.addStat(new Stat(StatType.Strength, StatBonusType.Pure, 10, ""));

            // Assert
            Assert.AreEqual(baseLife + 1, being.stats.maxLife);
        }

        [Test]
        public void WhenBeingTakeDamage_ThenTheBeingLifeIsReducedByTheDamageValue()
        {
            // assign
            float damage = 10f;
            float baseLife = being.stats.maxLife;

            // Act
            being.takeDamage(damage);

            // Assert
            Assert.AreEqual(baseLife - damage, being.currentLife);
        }

        [Test]
        public void WhenBeingIsHealed_ThenTheBeingLifeIsIncreaseByThehealingValue()
        {
            // assign
            float damage = 10f;
            float healValue = 10f;
            float baseLife = being.stats.maxLife;

            // Act
            being.takeDamage(damage);
            being.heal(healValue);

            // Assert
            Assert.AreEqual(baseLife, being.currentLife);
        }

        [Test]
        public void WhenBeingIsHealedAndLifeIsMaxedOut_ThenTheBeingLifeDontChange()
        {
            // assign
            float healValue = 10f;
            float baseLife = being.stats.maxLife;

            // Act
            being.heal(healValue);

            // Assert
            Assert.AreEqual(baseLife, being.currentLife);
        }

        [Test]
        public void WhenBeingLifeIsZeroOrLess_ThenBeingIsDead()
        {
            // assign
            float damage = being.stats.maxLife;

            // act
            being.takeDamage(damage);

            // assert
            Assert.IsTrue(being.isDead());
        }
        #endregion

        #region TearDown
        [TearDown]
        public void removeGameManagerInstance()
        {
            GameManager.instance = null;
            being = null;
        }
        #endregion
    }
}
