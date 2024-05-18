using Domain.Abstractions;

namespace Domain.Clients;

public sealed record ClientCreatedDomainEvent(Guid Id) : IDomainEvent;
