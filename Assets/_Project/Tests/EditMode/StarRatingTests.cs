using NUnit.Framework;
using LanternTrail.Core;

namespace LanternTrail.Tests.EditMode
{
    [TestFixture]
    public class StarRatingTests
    {
        [Test]
        [TestCase(0, ExpectedResult = 1)]
        [TestCase(1, ExpectedResult = 2)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(3, ExpectedResult = 3)]
        [TestCase(5, ExpectedResult = 3)]
        public int Calculate_ReturnsCorrectStars_BasedOnGoldenLanternsFound(int goldenCount)
        {
            return StarRating.Calculate(goldenCount);
        }

        [Test]
        public void Calculate_ZeroGoldenLanterns_AlwaysReturnsOneStar()
        {
            int stars = StarRating.Calculate(0);
            Assert.AreEqual(1, stars, "Zero golden lanterns must award exactly 1 star.");
        }

        [Test]
        public void Calculate_AllThreeGoldenLanterns_AlwaysReturnsThreeStars()
        {
            int stars = StarRating.Calculate(3);
            Assert.AreEqual(3, stars, "Finding all 3 golden lanterns must award 3 stars.");
        }
    }
}
