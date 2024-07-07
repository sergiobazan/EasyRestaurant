using Domain.Abstractions;
using Domain.Shared;

namespace Domain.Clients;

public sealed class Client : Entity
{
    private Client() { }
    private Client(Guid id,
        Email email,
        Name name,
        Phone phone,
        Gender gender, 
        string identityId)
        : base(id)
    { 
        Email = email;
        Name = name;
        Phone = phone;
        Gender = gender;
        IdentityId = identityId;
    }

    public Name Name { get; private set; }
    public Phone Phone { get; private set; }
    public Gender Gender { get; private set; }
    public string IdentityId { get; }
    public Email Email { get; private set; }

    public static Result<Client> Create(Email email, Name name, Phone phone, Gender gender, string identityId)
    {
        var client = new Client(Guid.NewGuid(), email, name, phone, gender, identityId);

        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));

        return client;
    }

}
