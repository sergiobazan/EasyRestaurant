using Domain.Abstractions;

namespace Domain.Orders.DomainEvents;

public sealed record OrderCreatedDomainEvent(Guid Id) : IDomainEvent;