namespace Roydl.Test
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class AlphaNumericComparerTests
    {
        private static readonly string[] ExpectedArray =
        {
            "Alpha111",
            "Alpha1150",
            "Alpha10000",
            "Foxtrot111",
            "Foxtrot1150",
            "Foxtrot10000",
            "Oscar111",
            "Oscar1150",
            "Oscar10000"
        };

        private static readonly string[] TestArray =
        {
            "Alpha10000",
            "Alpha111",
            "Alpha1150",
            "Foxtrot10000",
            "Foxtrot111",
            "Foxtrot1150",
            "Oscar10000",
            "Oscar111",
            "Oscar1150"
        };

        [Test]
        [TestCase(TestOf = typeof(AlphaNumericComparer<string>))]
        public void ArrayTest()
        {
            var comparer = new AlphaNumericComparer<string>();
            var sorted = TestArray.OrderBy(x => x, comparer).ToArray();
            Assert.AreEqual(ExpectedArray, sorted);
        }
    }
}
