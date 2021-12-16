using NSSolver;
using Xunit;

namespace NSSolverTests {
    public class NSConverterTests {

        [Fact]
        public void ConvertionTest() {
            //Assert.Equal("100101101", NSConverter.FromSomethingToSomethingNS("301", 10, 2));
            Assert.Equal("19F", NSConverter.FromSomethingToSomethingNS("415", 10, 16));
        }
    }
}