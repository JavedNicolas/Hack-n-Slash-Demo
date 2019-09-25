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
        #region SetUp
        [SetUp]
        public void setItem()
        {
            item = new Item("potion", null, new GameObject(), true, true, 10, true, TargetType.AnyItem, new List<ItemEffectAndValue>());
        }
        #endregion

        #region Tests

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
