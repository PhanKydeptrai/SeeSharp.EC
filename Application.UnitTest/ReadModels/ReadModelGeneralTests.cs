using Domain.Database.PostgreSQL.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class ReadModelGeneralTests
{
    [Fact]
    public void AllReadModels_Should_Have_Id_Property()
    {
        // Arrange
        var readModelTypes = GetAllReadModelTypes();

        // Act & Assert
        foreach (var modelType in readModelTypes)
        {
            var idPropertyName = $"{modelType.Name.Replace("ReadModel", "")}Id";
            var idProperty = modelType.GetProperty(idPropertyName);
            
            Assert.NotNull(idProperty);
            Assert.Equal(typeof(Ulid), idProperty.PropertyType);
        }
    }

    [Fact]
    public void AllReadModels_Properties_Should_Have_Public_Getters_And_Setters()
    {
        // Arrange
        var readModelTypes = GetAllReadModelTypes();

        // Act & Assert
        foreach (var modelType in readModelTypes)
        {
            var properties = modelType.GetProperties();
            
            foreach (var property in properties)
            {
                Assert.True(property.CanRead);
                Assert.True(property.CanWrite);
            }
        }
    }

    private List<Type> GetAllReadModelTypes()
    {
        var readModelNamespace = typeof(UserReadModel).Namespace;
        var assembly = Assembly.GetAssembly(typeof(UserReadModel));
        
        return assembly.GetTypes()
            .Where(t => t.Namespace == readModelNamespace && t.Name.EndsWith("ReadModel"))
            .ToList();
    }
} 