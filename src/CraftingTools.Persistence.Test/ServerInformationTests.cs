using CraftingTools.Shared;
using NUnit.Framework;

namespace CraftingTools.Persistence.Test;

/// <summary>
/// Tests for <see cref="ServerInformation"/>.
/// </summary>
internal static class ServerInformationTests
{
    [Test]
    public static void FromParameters_ValidateBehavior()
    {
        // use case: invalid parameters
        {
            var result = ServerInformation.FromParameter(
                host: null,
                port: -1,
                userId: null,
                password: null,
                resultId: "invalid_parameters");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Unable to create server information."));
            Assert.That(result.Id, Is.EqualTo(expected: "invalid_parameters"));
            var exception = result.Error.Exception as RailwayResultFailureException;
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception!.Failures.Count, Is.EqualTo(expected: 4));
            var failure = exception.Failures[index: 0];
            Assert.That(failure.Status, Is.EqualTo(RailwayResultStatus.Failure));
        }

        // use case: valid parameters
        {
            var result = ServerInformation.FromParameter(
                host: "host",
                port: 1234,
                userId: "userId",
                password: "password");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            var information = result.Unwrap();
            Assert.That(information, Is.Not.Null);
            Assert.That(information.Host, Is.EqualTo(expected: "host"));
            Assert.That(information.Port, Is.EqualTo(expected: 1234));
            Assert.That(information.UserId, Is.EqualTo(expected: "userId"));
            Assert.That(information.Password, Is.EqualTo(expected: "password"));
        }
    }
}