using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EffectTests
    {
        EffectForTest effect;

        #region SetUp
        [SetUp]
        public void setEffect()
        {
            effect = (EffectForTest)ScriptableObject.CreateInstance(typeof(EffectForTest));
        }
        #endregion

        #region Tests
        [Test]
        public void WhenEffectTargetAndSenderAreNotOnTheSameTeamAndTheEffectApplyToEnemy_ThenWeCanUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Enemy;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 1;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnTheSameTeamAndTheEffectApplyToEnemy_ThenWeCantUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Enemy;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(!canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnTheSameTeamAndTheEffectApplyToAlly_ThenWeCanUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Allies;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnNotTheSameTeamAndTheEffectApplyToAlly_ThenWeCantUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Allies;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 1;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(!canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreTheSameAndTheEffectApplyToSelf_ThenWeCanUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Self;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            PlayerBehavior enemyBehavior = playerBehavior;
            enemyBehavior.teamID = 0;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreNotTheSameAndTheEffectApplyToSelf_ThenWeCantUseTheEffect()
        {
            // assign
            effect.effectTargetType = EffectTargetType.Self;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = effect.canBeUsed(playerBehavior, enemyBehavior.gameObject, 10);

            // assert
            Assert.IsTrue(!canUse);
        }
        #endregion

        #region TearDowns
        [TearDown]
        public void resetEffect()
        {
            effect = null;
        }
        #endregion
    }
}
