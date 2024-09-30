// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Threading.Tasks;
// using DVP.Tasks.Infraestructure.Services;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Graph;
// using Moq;
// using Newtonsoft.Json;
// using Xunit;

// namespace DVP.Tasks.Tests.Services
// {
//     public class AzureActiveDirectoryServiceTests
//     {
//         private readonly Mock<IConfiguration> _configurationMock;
//         private readonly AzureActiveDirectoryService _service;

//         public AzureActiveDirectoryServiceTests()
//         {
//             _configurationMock = new Mock<IConfiguration>();
//             _configurationMock.Setup(c => c.GetValue<string>("Azure:TenantId")).Returns("tenant-id");
//             _configurationMock.Setup(c => c.GetValue<string>("Azure:ClientId")).Returns("client-id");
//             _configurationMock.Setup(c => c.GetValue<string>("Azure:ClientSecret")).Returns("client-secret");
//             _configurationMock.Setup(c => c.GetValue<string>("Azure:Domain")).Returns("domain.com");

//             _service = new AzureActiveDirectoryService(_configurationMock.Object);
//         }

//         [Fact]
//         public async Task CreateAadUser_ShouldReturnUserId_WhenUserCreatedSuccessfully()
//         {
//             // Arrange
//             var name = "Test User";
//             var email = "test@domain.com";
//             var password = "Password123!";
//             var userId = Guid.NewGuid().ToString();

//             var user = new Microsoft.Graph.Models.User
//             {
//                 Id = userId // Simulate a valid user id
//             };

//             // Create a mock GraphServiceClient
//             var graphClientMock = new Mock<GraphServiceClient>();

//             // Setup the mock response for User creation
//             graphClientMock.Setup(g => g.Users
//                 .PostAsync(It.IsAny<Microsoft.Graph.Models.User>(), It.IsAny<Action<RequestConfiguration<DefaultQueryParameters>>>()))
//                 .ReturnsAsync(user);

//             // Act
//             var result = await _service.CreateAadUser(name, email, password);
            
//             // Assert
//             Assert.NotEqual(Guid.Empty, result);
//         }

//         [Fact]
//         public async Task CreateAadUser_ShouldReturnEmptyGuid_WhenUserCreationFails()
//         {
//             // Arrange
//             var name = "Test User";
//             var email = "test@domain.com";
//             var password = "Password123!";

//             // Create a mock GraphServiceClient
//             var graphClientMock = new Mock<GraphServiceClient>();

//             // Setup to throw an exception for user creation
//             graphClientMock.Setup(g => g.Users
//                 .PostAsync(It.IsAny<Microsoft.Graph.Models.User>(), It.IsAny<Action<RequestConfiguration<DefaultQueryParameters>>>()))
//                 .ThrowsAsync(new ServiceException(new ErrorResponse { Message = "User creation failed" }));

//             // Act
//             var result = await _service.CreateAadUser(name, email, password);
            
//             // Assert
//             Assert.Equal(Guid.Empty, result);
//         }


//     }
// }
