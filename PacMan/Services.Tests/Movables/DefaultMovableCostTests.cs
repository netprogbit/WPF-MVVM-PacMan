using Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Movables;
using System.Drawing;

namespace Services.Tests.Movables
{
    [TestClass]
    public class DefaultMovableCostTests
    {
        private DefaultMovableCost _gmc;

        [TestInitialize]
        public void TestInitialize()
        {
            Occupation[,] matrix =
            {
                { Occupation.Empty, Occupation.Wall },
                { Occupation.Empty, Occupation.Empty }
            };

            _gmc = new DefaultMovableCost(matrix);
        }

        [TestMethod]
        public void Move_11StartAnd00Goal_10Result()
        {
            Point result = _gmc.Move(new Point(1, 1), new Point(0, 0));

            Assert.AreEqual(new Point(1, 0), result);
        }
    }
}
