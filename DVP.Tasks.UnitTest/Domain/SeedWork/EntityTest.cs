// using System;
// using System.Collections.Generic;
// using Moq;
// using Xunit;
// using DVP.Tasks.Domain.SeedWork;
// using MediatR;

// namespace DVP.Tasks.Tests.Domain.SeedWork
// {
//     public class EntityTests
//     {
//         private class TestEntity : Entity
//         {
//             public TestEntity(Guid id)
//             {
//                 Id = id;
//             }

//             public void SetId(Guid id)
//             {
//                 Id = id; // For testing purposes
//             }
//         }

//         [Fact]
//         public void AddDomainEvent_ShouldAddEvent()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.NewGuid());
//             var mockEvent = new Mock<INotification>();

//             // Act
//             entity.AddDomainEvent(mockEvent.Object);

//             // Assert
//             Assert.NotNull(entity.DomainEvents);
//             Assert.Single(entity.DomainEvents);
//             Assert.Contains(mockEvent.Object, entity.DomainEvents);
//         }

//         [Fact]
//         public void RemoveDomainEvent_ShouldRemoveEvent()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.NewGuid());
//             var mockEvent = new Mock<INotification>();
//             entity.AddDomainEvent(mockEvent.Object);

//             // Act
//             entity.RemoveDomainEvent(mockEvent.Object);

//             // Assert
//             Assert.Empty(entity.DomainEvents);
//         }

//         [Fact]
//         public void ClearDomainEvents_ShouldRemoveAllEvents()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.NewGuid());
//             var mockEvent1 = new Mock<INotification>();
//             var mockEvent2 = new Mock<INotification>();
//             entity.AddDomainEvent(mockEvent1.Object);
//             entity.AddDomainEvent(mockEvent2.Object);

//             // Act
//             entity.ClearDomainEvents();

//             // Assert
//             Assert.Empty(entity.DomainEvents);
//         }

//         [Fact]
//         public void IsTransient_ShouldReturnTrue_WhenIdIsDefaultGuid()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.Empty);

//             // Act
//             var result = entity.IsTransient();

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public void IsTransient_ShouldReturnFalse_WhenIdIsNotDefaultGuid()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.NewGuid());

//             // Act
//             var result = entity.IsTransient();

//             // Assert
//             Assert.False(result);
//         }

//         [Fact]
//         public void Equals_ShouldReturnTrue_ForSameEntityReference()
//         {
//             // Arrange
//             var id = Guid.NewGuid();
//             var entity1 = new TestEntity(id);
//             var entity2 = entity1;

//             // Act
//             var result = entity1.Equals(entity2);

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public void Equals_ShouldReturnFalse_ForDifferentTypes()
//         {
//             // Arrange
//             var entity = new TestEntity(Guid.NewGuid());

//             // Act
//             var result = entity.Equals(new object());

//             // Assert
//             Assert.False(result);
//         }

//         [Fact]
//         public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
//         {
//             // Arrange
//             var id = Guid.NewGuid();
//             var entity1 = new TestEntity(id);
//             var entity2 = new TestEntity(id);

//             // Act
//             var result = entity1.Equals(entity2);

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public void Equals_ShouldReturnFalse_WhenIdsAreDifferent()
//         {
//             // Arrange
//             var entity1 = new TestEntity(Guid.NewGuid());
//             var entity2 = new TestEntity(Guid.NewGuid());

//             // Act
//             var result = entity1.Equals(entity2);

//             // Assert
//             Assert.False(result);
//         }

//         [Fact]
//         public void GetHashCode_ShouldReturnSameHashCode_ForSameId()
//         {
//             // Arrange
//             var id = Guid.NewGuid();
//             var entity1 = new TestEntity(id);
//             var entity2 = new TestEntity(id);

//             // Act
//             var hashCode1 = entity1.GetHashCode();
//             var hashCode2 = entity2.GetHashCode();

//             // Assert
//             Assert.Equal(hashCode1, hashCode2);
//         }

//         [Fact]
//         public void GetHashCode_ShouldReturnDifferentHashCode_ForDifferentIds()
//         {
//             // Arrange
//             var entity1 = new TestEntity(Guid.NewGuid());
//             var entity2 = new TestEntity(Guid.NewGuid());

//             // Act
//             var hashCode1 = entity1.GetHashCode();
//             var hashCode2 = entity2.GetHashCode();

//             // Assert
//             Assert.NotEqual(hashCode1, hashCode2);
//         }

//         [Fact]
//         public void OperatorEquals_ShouldReturnTrue_WhenEntitiesAreEqual()
//         {
//             // Arrange
//             var id = Guid.NewGuid();
//             var entity1 = new TestEntity(id);
//             var entity2 = new TestEntity(id);

//             // Act
//             var result = entity1 == entity2;

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public void OperatorNotEquals_ShouldReturnTrue_WhenEntitiesAreNotEqual()
//         {
//             // Arrange
//             var entity1 = new TestEntity(Guid.NewGuid());
//             var entity2 = new TestEntity(Guid.NewGuid());

//             // Act
//             var result = entity1 != entity2;

//             // Assert
//             Assert.True(result);
//         }
//     }
// }
