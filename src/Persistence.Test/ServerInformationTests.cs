using NUnit.Framework;
using SleepingBearSystems.Tools.Railway;
using SleepingBearSystems.Tools.Testing;

namespace SleepingBearSystem.CraftingTools.Persistence.Test;

/// <summary>
/// Tests for <see cref="ServerInformation"/>.
/// </summary>
internal static class ServerInformationTests
{
    [Test]
    public static void FromParameters_ValidateBehavior()
    {
        var log = new List<string>();
        var logger = TestLogger.Create(log, timeStampFormat: string.Empty);

        // use case: invalid parameters
        {
            var result = ServerInformation.FromParameter(
                host: null,
                port: -1,
                userId: null,
                password: null,
                resultId: "invalid_parameters");
            result.LogResult(logger, (localLogger, localInformation) => { });
        }

        // use case: valid parameters
        {
            var result = ServerInformation.FromParameter(
                host: "host",
                port: 1234,
                userId: "userId",
                password: "password");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            var information = result.Unwrap();
            Assert.That(information, Is.Not.Null);
            Assert.That(information.Host, Is.EqualTo(expected: "host"));
            Assert.That(information.Port, Is.EqualTo(expected: 1234));
            Assert.That(information.UserId, Is.EqualTo(expected: "userId"));
            Assert.That(information.Password, Is.EqualTo(expected: "password"));
        }
    }
}
