namespace Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; init; }
    private readonly List<IDomainEvent> domainEvents = new();

    public Entity(Guid id)
    {
        Id = id;
    }
    public Entity()
    {
        
    }

    public List<IDomainEvent> DomainEvents => domainEvents.ToList();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }
}
