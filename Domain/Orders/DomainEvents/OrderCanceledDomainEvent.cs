using Domain.Abstractions;

namespace Domain.Orders.DomainEvents;

public sealed record OrderCanceledDomainEvent(Guid Id) : IDomainEvent;
