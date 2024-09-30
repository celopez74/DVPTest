using System.Linq;
using Xunit;
using DVP.Tasks.Domain.SeedWork;
using System.Collections.Generic;

public class TestValueObject : ValueObject
{
    public string Property1 { get; }
    public int Property2 { get; }

    public TestValueObject(string property1, int property2)
    {
        Property1 = property1;
        Property2 = property2;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Property1;
        yield return Property2;
    }
}

namespace DVP.Tasks.Tests.Domain.SeedWork
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equals_ShouldReturnTrue_WhenObjectsAreEqual()
        {
            // Arrange
            var obj1 = new TestValueObject("Test", 123);
            var obj2 = new TestValueObject("Test", 123);

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenObjectsAreNotEqual()
        {
            // Arrange
            var obj1 = new TestValueObject("Test", 123);
            var obj2 = new TestValueObject("Different", 456);

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHashCode_ForEqualObjects()
        {
            // Arrange
            var obj1 = new TestValueObject("Test", 123);
            var obj2 = new TestValueObject("Test", 123);

            // Act
            var hashCode1 = obj1.GetHashCode();
            var hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHashCodes_ForNonEqualObjects()
        {
            // Arrange
            var obj1 = new TestValueObject("Test", 123);
            var obj2 = new TestValueObject("Different", 456);

            // Act
            var hashCode1 = obj1.GetHashCode();
            var hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void GetCopy_ShouldReturnCopyOfObject()
        {
            // Arrange
            var obj = new TestValueObject("Test", 123);

            // Act
            var copy = obj.GetCopy();

            // Assert
            Assert.NotSame(obj, copy);
            Assert.Equal(obj, copy);
        }
    }
}
