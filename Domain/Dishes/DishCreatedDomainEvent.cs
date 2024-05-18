using Domain.Abstractions;

namespace Domain.Dishes;

public sealed record DishCreatedDomainEvent(Guid Id) : IDomainEvent;