using System;
using Application.Abstractions.Messaging;

namespace Application.Features.OrderFeature.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : ICommand;
