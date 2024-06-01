using Domain.Abstractions;
using Domain.Shared;

namespace Domain.Clients;

public sealed class Client : Entity
{
    private Client() { }
    private Client(
        Guid id,
        Name name,
        Phone phone,
        Gender gender)
        : base(id)
    { 
        Name = name;
        Phone = phone;
        Gender = gender;
    }

    public Name Name { get; private set; }
    public Phone Phone { get; private set; }
    public Gender Gender { get; private set; }

    public static Result<Client> Create(Name name, Phone phone, Gender gender)
    {
        var client = new Client(Guid.NewGuid(), name, phone, gender);

        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));

        return client;
    }

    public Result<Client> Update(Name name, Phone phone)
    {
        Name = name;
        Phone = phone;

        return this;
    }

}
