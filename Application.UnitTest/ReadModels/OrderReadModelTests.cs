using Domain.Database.PostgreSQL.ReadModels;
using Domain.Entities.Bills;
using Domain.Entities.Orders;
using System.Collections.Generic;
using Xunit;

namespace SeeSharp.EC.Application.UnitTest.ReadModels;

public class OrderReadModelTests
{
    [Fact]
    public void OrderReadModel_Properties_Should_Be_Set_Correctly()
    {
        // Arrange
        var orderId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        var orderTransactionId = Ulid.NewUlid();
        var total = 1000.0m;
        var paymentStatus = PaymentMethod.Cash;
        var orderStatus = OrderStatus.New;

        // Act
        var orderReadModel = new OrderReadModel
        {
            OrderId = orderId,
            CustomerId = customerId,
            OrderTransactionId = orderTransactionId,
            Total = total,
            PaymentStatus = paymentStatus,
            OrderStatus = orderStatus,
            OrderDetailReadModels = new List<OrderDetailReadModel>()
        };

        // Assert
        Assert.Equal(orderId, orderReadModel.OrderId);
        Assert.Equal(customerId, orderReadModel.CustomerId);
        Assert.Equal(orderTransactionId, orderReadModel.OrderTransactionId);
        Assert.Equal(total, orderReadModel.Total);
        Assert.Equal(paymentStatus, orderReadModel.PaymentStatus);
        Assert.Equal(orderStatus, orderReadModel.OrderStatus);
        Assert.NotNull(orderReadModel.OrderDetailReadModels);
        Assert.Empty(orderReadModel.OrderDetailReadModels);
    }

    [Fact]
    public void OrderReadModel_Can_Have_Related_Entities()
    {
        // Arrange
        var orderId = Ulid.NewUlid();
        var customerId = Ulid.NewUlid();
        var customerReadModel = new CustomerReadModel { CustomerId = customerId };
        
        var orderDetailId = Ulid.NewUlid();
        var orderDetail = new OrderDetailReadModel { OrderDetailId = orderDetailId };
        
        var billId = Ulid.NewUlid();
        var billReadModel = new BillReadModel { BillId = billId };
        
        var feedbackId = Ulid.NewUlid();
        var feedbackReadModel = new FeedbackReadModel { FeedbackId = feedbackId };
        
        var transactionId = Ulid.NewUlid();
        var transactionReadModel = new OrderTransactionReadModel { OrderTransactionId = transactionId };

        // Act
        var orderReadModel = new OrderReadModel
        {
            OrderId = orderId,
            CustomerId = customerId,
            CustomerReadModel = customerReadModel,
            BillReadModel = billReadModel,
            FeedbackReadModel = feedbackReadModel,
            OrderTransactionReadModel = transactionReadModel,
            OrderDetailReadModels = new List<OrderDetailReadModel> { orderDetail }
        };

        // Assert
        Assert.NotNull(orderReadModel.CustomerReadModel);
        Assert.Equal(customerId, orderReadModel.CustomerReadModel.CustomerId);
        
        Assert.NotNull(orderReadModel.BillReadModel);
        Assert.Equal(billId, orderReadModel.BillReadModel.BillId);
        
        Assert.NotNull(orderReadModel.FeedbackReadModel);
        Assert.Equal(feedbackId, orderReadModel.FeedbackReadModel.FeedbackId);
        
        Assert.NotNull(orderReadModel.OrderTransactionReadModel);
        Assert.Equal(transactionId, orderReadModel.OrderTransactionReadModel.OrderTransactionId);
        
        Assert.NotNull(orderReadModel.OrderDetailReadModels);
        Assert.Single(orderReadModel.OrderDetailReadModels);
        Assert.Equal(orderDetailId, orderReadModel.OrderDetailReadModels.First().OrderDetailId);
    }

    [Fact]
    public void OrderReadModel_Optional_Properties_Can_Be_Null()
    {
        // Arrange & Act
        var orderReadModel = new OrderReadModel
        {
            OrderId = Ulid.NewUlid(),
            CustomerId = Ulid.NewUlid(),
            Total = 1000.0m,
            PaymentStatus = PaymentMethod.Cash,
            OrderStatus = OrderStatus.New,
            OrderTransactionId = Ulid.NewUlid(),
            CustomerReadModel = new CustomerReadModel(),
            // Optional properties not set
            BillReadModel = null,
            FeedbackReadModel = null,
            OrderTransactionReadModel = null
        };

        // Assert
        Assert.Null(orderReadModel.BillReadModel);
        Assert.Null(orderReadModel.FeedbackReadModel);
        Assert.Null(orderReadModel.OrderTransactionReadModel);
        Assert.NotNull(orderReadModel.OrderDetailReadModels);
        Assert.Empty(orderReadModel.OrderDetailReadModels);
    }
} 