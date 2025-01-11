using Cindra;

namespace CindraTest
{
    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        [DataRow(3, 0, 1, 3)] // head jab red
        [DataRow(2, 0, 2, 2)] // head jab yellow
        [DataRow(1, 0, 3, 2)] // head jab blue; can do 2 with daggers
        [DataRow(4, 1, 1, 1)] // leg tap red; can do 1 with dagger
        [DataRow(3, 1, 2, 2)] // leg tap yellow; can do 2 with daggers
        [DataRow(2, 1, 3, 2)] // leg tap blue; can do 2 with daggers
        public void OneCard(int power, int cost, int pitch, int expected)
        {
            var cards = new[]
            {
                new Card(power, cost, pitch, goAgain: true),
            };

            var actual = Calculator.GetResult(cards)?.OverallMax;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3, 0, 1, 3, 0, 1, 6, DisplayName = "2x head jab red")]
        [DataRow(3, 0, 1, 1, 0, 3, 5, DisplayName = "head jab red + head jab blue")]
        [DataRow(4, 1, 1, 1, 0, 3, 6, DisplayName = "leg tap red + head jab blue")]
        public void TwoCards_BothGoAgain(int power1, int cost1, int pitch1, int power2, int cost2, int pitch2, int expected)
        {
            var cards = new[]
            {
                new Card(power1, cost1, pitch1, goAgain: true),
                new Card(power2, cost2, pitch2, goAgain: true),
            };

            var actual = Calculator.GetResult(cards)?.OverallMax;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(4, 0, 1, 4, 0, 1, 5, DisplayName = "2x wounding blow red")]
        [DataRow(4, 0, 1, 2, 0, 3, 6, DisplayName = "wounding blow red + wounding blow blue")]
        public void TwoCards_BothWithoutGoAgain(int power1, int cost1, int pitch1, int power2, int cost2, int pitch2, int expected)
        {
            var cards = new[]
            {
                new Card(power1, cost1, pitch1, goAgain: false),
                new Card(power2, cost2, pitch2, goAgain: false),
            };

            var actual = Calculator.GetResult(cards)?.OverallMax;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(true, 12, DisplayName = "3x draconic head jab + blue")]
        [DataRow(false, 11, DisplayName = "3x non-draconic head jab + blue")]
        public void ThreeHeadJabsPlusBlue_DependsOnDraconic(bool draconic, int expected)
        {
            var cards = new[]
            {
                new Card(3, 0, 1, goAgain: true, draconic),
                new Card(3, 0, 1, goAgain: true, draconic),
                new Card(3, 0, 1, goAgain: true, draconic),
                new Card(1, 0, 3, goAgain: true, draconic),
            };

            var actual = Calculator.GetResult(cards)?.OverallMax;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FourCards()
        {
            var cards = new[]
            {
                new Card(3, 0, 1, goAgain: true),
                new Card(3, 0, 1, goAgain: true),
                new Card(3, 0, 1, goAgain: true),
                new Card(3, 0, 1, goAgain: true),
            };

            var actual = Calculator.GetResult(cards)?.OverallMax;
            Assert.AreEqual(12, actual);
        }

        [TestMethod]
        public void DaggersAtEnd_ThreeDraconicCards()
        {
            var cards = new[]
            {
                new Card(3, 0, 1, goAgain: true),
                new Card(3, 0, 1, goAgain: true),
                new Card(3, 0, 1, goAgain: true),
            };

            var actual = Calculator.GetResult(cards)?.DaggersAtEnd[0];
            Assert.AreEqual(2, actual);
        }
    }
}