using Domain.Abstractions;

namespace Domain.Orders;

public sealed record OrderCreatedDomainEvent(Guid Id) : IDomainEvent;