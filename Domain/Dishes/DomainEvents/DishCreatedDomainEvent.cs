using Domain.Abstractions;

namespace Domain.Dishes.DomainEvents;

public sealed record DishCreatedDomainEvent(Guid Id) : IDomainEvent;