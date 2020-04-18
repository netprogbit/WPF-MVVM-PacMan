using Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Factories;

namespace Services.Tests.Factories
{
    [TestClass]
    public class MazeFactoryTests
    {
        private MazeFactory _mf;

        [TestInitialize]
        public void TestInitialize()
        {
            _mf = new MazeFactory();
        }

        [TestMethod]
        public void Create_10Width10Height_WallsNoMore40percentResult()
        {
            Occupation[,] matrix = _mf.Create(10, 10);

            int amountWall = 0;

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == Occupation.Wall)
                        amountWall++;
                }
            }

            int fillPercent = amountWall * 100 / matrix.Length;

            bool result = fillPercent <= 40;

            Assert.AreEqual(true, result);
        }
    }
}
