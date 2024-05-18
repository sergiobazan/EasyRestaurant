using Domain.Abstractions;
using Domain.Clients;
using FluentAssertions;

namespace Domain.Test.Clients;

public class GenderTest
{
    [Theory]
    [InlineData("M")]
    [InlineData("F")]
    public void Create_Should_ReturnGender_WhenParametersAreValid(string value)
    {
        var gender = Gender.Create(value);

        gender.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_Should_ThrowArgumentException_WhenParametersAreNullOrEmpty(string value)
    {
        Result<Gender> Action() => Gender.Create(value);

        //Assert
        FluentActions.Invoking(Action).Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("S")]
    public void Create_Should_ThrowArgumentException_WhenParameterIsInvalid(string value)
    {
        Result<Gender> gender = Gender.Create(value);

        //Assert
        gender.IsFailure.Should().BeTrue();
    }
}
