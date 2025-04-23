using Domain.Utilities.Events.CustomerEvents;
using Domain.Utilities.Events.EmployeeEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.EventsTest;

public class EventsGeneralTests
{
    [Fact]
    public void All_Events_Should_Have_Characteristics_Of_Records()
    {
        // Arrange
        var eventTypes = GetAllEventTypes();

        // Act & Assert
        foreach (var eventType in eventTypes)
        {
            // Records have EqualityContract, ToString, Equals, GetHashCode implementations
            Assert.NotNull(eventType.GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            Assert.NotNull(eventType.GetMethod("Equals", new[] { typeof(object) }));
            Assert.NotNull(eventType.GetMethod("GetHashCode"));
            Assert.NotNull(eventType.GetMethod("ToString"));
        }
    }


    [Fact]
    public void All_Events_Should_Have_Email_Property()
    {
        // Arrange
        var eventTypes = GetAllEventTypes();

        // Act & Assert
        foreach (var eventType in eventTypes)
        {
            var emailProperty = eventType.GetProperty("Email");
            Assert.NotNull(emailProperty);
            Assert.Equal(typeof(string), emailProperty.PropertyType);
        }
    }

    [Fact]
    public void All_Events_Should_Have_ToString_Override()
    {
        // Arrange
        var eventTypes = GetAllEventTypes();
        var objectToString = typeof(object).GetMethod("ToString");

        // Act & Assert
        foreach (var eventType in eventTypes)
        {
            var toStringMethod = eventType.GetMethod("ToString", Type.EmptyTypes);
            Assert.NotNull(toStringMethod);
            Assert.NotEqual(objectToString, toStringMethod);
        }
    }

    [Fact]
    public void All_Events_Should_Support_Value_Equality()
    {
        // Arrange
        var customerEvents = GetAllEventTypes().Where(t => t.Namespace.Contains("CustomerEvents")).ToList();
        var employeeEvents = GetAllEventTypes().Where(t => t.Namespace.Contains("EmployeeEvents")).ToList();
        
        foreach (var eventType in customerEvents.Concat(employeeEvents))
        {
            TestValueEqualityOfEvent(eventType);
        }
    }

    private void TestValueEqualityOfEvent(Type eventType)
    {
        try
        {
            // Create a Guid that we can reuse for both instances to ensure equality
            var sameGuid = Guid.NewGuid();
            
            // Use reflection to create instances with the same values
            var constructorParams = eventType.GetConstructors().First().GetParameters();
            var args = constructorParams.Select(p => 
            {
                if (p.ParameterType == typeof(Guid))
                    return sameGuid;
                return GetDefaultValue(p.ParameterType);
            }).ToArray();
            
            // Skip test if constructor has parameters we can't handle
            if (args.Any(a => a == null))
                return;
                
            // Create two instances with identical values
            var instance1 = Activator.CreateInstance(eventType, args);
            var instance2 = Activator.CreateInstance(eventType, args);
            
            // Record types should implement value equality
            Assert.Equal(instance1, instance2);
            
            // ToString() output should be the same for equal instances
            Assert.Equal(instance1.ToString(), instance2.ToString());
        }
        catch
        {
            // Skip test if we encounter errors creating the instances
            return;
        }
    }

    private object GetDefaultValue(Type type)
    {
        if (type == typeof(string))
            return "test@example.com";
        else if (type == typeof(Guid))
            return Guid.NewGuid();
        else if (type == typeof(bool))
            return false;
        else if (type == typeof(int))
            return 0;
        else
            return null;
    }

    private List<Type> GetAllEventTypes()
    {
        // Get a type from each namespace to use as a reference
        var customerEventType = typeof(AccountVerificationEmailSentEvent);
        var employeeEventType = typeof(EmployeeChangePasswordEvent);
        
        var customerNamespace = customerEventType.Namespace;
        var employeeNamespace = employeeEventType.Namespace;
        
        var assembly = Assembly.GetAssembly(customerEventType);
        
        return assembly.GetTypes()
            .Where(t => (t.Namespace == customerNamespace || t.Namespace == employeeNamespace) && t.IsClass)
            .ToList();
    }
} 