using Domain.Abstractions;

namespace Domain.Clients;

public static class ClientErrors
{
    public static Error OnlyTwoGenders => new(
        "Gender.OnlyTwoGenders", "Genders must be Male(M) or Female(F)");

    public static Error InvalidPrefix => new(
        "Phone.InvalidPrefix", "The prefix does not match a valid country code");

    public static Error InvalidPhoneNumber => new(
       "Phone.InvalidPhoneNumber", "The value does not match a valid phone number");

    public static Error ClientNotFound(Guid id) => new(
        "Client.ClientNotFound", $"The client with the Id = {id} was not found.");
}
