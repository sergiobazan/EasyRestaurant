using Domain.Abstractions;

namespace Domain.Dishes.DomainEvents;

public sealed record DishServedDomainEvent(Guid Id) : IDomainEvent;
