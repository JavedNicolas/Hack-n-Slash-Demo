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
        #region SetUp
        [SetUp]
        public void setGameManager()
        {
            GameManager.instance = new GameObject().AddComponent<GameManager>();
            GameManager.instance.abilityDatabase = Resources.Load<AbilityDatabase>(DatabaseConstant.abilityDatabasePath);
            GameManager.instance.resourcesList = Resources.Load<DatabaseResourcesList>(ScriptableObjectConstant.resourceListPath);
        }
        #endregion

        #region Tests

        #endregion

        #region TearDowns

        #endregion
    }
}

