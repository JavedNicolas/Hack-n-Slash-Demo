using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AbilityTests
    {
        Ability ability;
        AbilityAttributs mockUpAbilityAttribut;
        #region SetUp
        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.loadDatabases();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.resourcesList = Resources.Load<ResourcesList>(ScriptableObjectConstant.resourceListPath);
        }

        [SetUp]
        public void setAbility()
        {
            mockUpAbilityAttribut = (AbilityAttributs)ScriptableObject.CreateInstance(typeof(AbilityAttributs));
            ability = (LightningBall)Activator.CreateInstance(typeof(LightningBall));
            ability.abilityAttributs = mockUpAbilityAttribut;
        }
        #endregion

        #region Tests
        [Test]
        public void WhenEffectTargetAndSenderAreNotOnTheSameTeamAndTheEffectApplyToEnemy_ThenWeCanUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Enemy;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 1;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnTheSameTeamAndTheEffectApplyToEnemy_ThenWeCantUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Enemy;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(!canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnTheSameTeamAndTheEffectApplyToAlly_ThenWeCanUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Allies;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreOnNotTheSameTeamAndTheEffectApplyToAlly_ThenWeCantUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Allies;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 1;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(!canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreTheSameAndTheEffectApplyToSelf_ThenWeCanUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Self;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            PlayerBehavior enemyBehavior = playerBehavior;
            enemyBehavior.teamID = 0;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(canUse);
        }

        [Test]
        public void WhenEffectTargetAndSenderAreNotTheSameAndTheEffectApplyToSelf_ThenWeCantUseTheEffect()
        {
            // assign
            mockUpAbilityAttribut.targetType = BeingTargetType.Self;
            PlayerBehavior playerBehavior = new GameObject().AddComponent<PlayerBehavior>();
            playerBehavior.being = new Player("test", 100, 1, 1, 1, 10, new List<int>(), null);
            playerBehavior.teamID = 0;
            EnemyBehavior enemyBehavior = new GameObject().AddComponent<EnemyBehavior>();
            enemyBehavior.being = new Enemy("enemy", 100, 2, 1, 10, 10, new List<int>(), new List<AbilityUsageFrequence>(), null, new List<Loot>(), 10);
            enemyBehavior.teamID = 0;

            // act
            bool canUse = ability.isCorrectTarget(playerBehavior, enemyBehavior);

            // assert
            Assert.IsTrue(!canUse);
        }
        #endregion

        #region TearDowns

        #endregion
    }
}

