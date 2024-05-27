using Domain.Abstractions;

namespace Domain.Menus.DomainEvents;

public sealed record MenuCreatedDomainEvent(Guid Id) : IDomainEvent;
