﻿using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Core.Tests
{
    [TestFixture]
    public class CommonHelperTests
    {
        [Test]
        public void Can_get_typed_value()
        {
            CommonHelper.To<int>("1000").ShouldBe<int>();
            CommonHelper.To<int>("1000").ShouldEqual(1000);
        }
    }
}
