using NUnit.Framework;
using BoilerPlate.Interfaces;
using BoilerPlate.Services;

namespace BoilerPlate.Tests.Unit
{
    public class Tests
    {
        private ITestService _testServiceSut;

        [SetUp]
        public void Setup()
        {
            _testServiceSut = new TestService();
        }

        [Test]
        public void Get_ShouldReturnTest()
        {
            // given
            var expected = "test";

            // when
            var result = _testServiceSut.Get();

            // then
            Assert.AreEqual(expected, result);
        }
    }
}