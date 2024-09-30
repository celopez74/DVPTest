using System;
using Xunit;

namespace DVP.Tasks.Domain.Exception.Tests
{
    public class DVPExceptionTests
    {
        [Fact]
        public void DVPException_DefaultConstructor_ShouldSetMessageToNotNull()
        {
            // Act
            var exception = new DVPException();

            // Assert
            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void DVPException_MessageConstructor_ShouldSetMessage()
        {
            // Arrange
            var message = "An error occurred.";

            // Act
            var exception = new DVPException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void DVPException_CodeAndMessageConstructor_ShouldSetCodeAndMessage()
        {
            // Arrange
            var code = 404;
            var message = "Entity not found.";

            // Act
            var exception = new DVPException(code, message);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(code, exception.Code);
        }
    }
}
