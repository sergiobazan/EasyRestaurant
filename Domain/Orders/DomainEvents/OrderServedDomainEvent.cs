using Domain.Abstractions;

namespace Domain.Orders.DomainEvents;

public sealed record OrderServedDomainEvent(Guid Id) : IDomainEvent;
