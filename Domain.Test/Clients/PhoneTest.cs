using Domain.Abstractions;
using Domain.Clients;
using FluentAssertions;

namespace Domain.Test.Clients;

public class PhoneTest
{
    [Theory]
    [InlineData("+5", "930787866")]
    [InlineData("+501", "930787866")]
    [InlineData("+71", "93078786")]
    [InlineData("+s1", "930787868")]
    public void Constructor_Should_ThrowArgumentException_WhenValuesAreInvalid(string prefix, string value)
    {
        Result<Phone> gender = Phone.Create(prefix, value);

        //Assert
        gender.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData("+51", "930787866")]
    public void Constructor_Should_CreateNumber_WhenValuesAreValid(string prefix, string value)
    {
        Result<Phone> phone = Phone.Create(prefix, value);

        //Assert
        phone.Value.Should().BeOfType<Phone>();
    }
}
