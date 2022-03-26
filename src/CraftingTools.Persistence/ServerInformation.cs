using System.Collections.Immutable;
using SleepingBearSystems.Common;
using SleepingBearSystems.Tools.Railway;

namespace CraftingTools.Persistence;

/// <summary>
/// Container class for a server connection.
/// </summary>
public sealed class ServerInformation : ValueObject<ServerInformation>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private ServerInformation(string host, int port, string userId, string password)
    {
        this.Host = host;
        this.Port = port;
        this.UserId = userId;
        this.Password = password;
    }

    public string Host { get; }

    public int Port { get; }

    public string UserId { get; }

    public string Password { get; }

    public static readonly ServerInformation None = new(string.Empty, port: 0, string.Empty, string.Empty);

    /// <summary>
    /// Factory method for creating a <see cref="ServerInformation"/> instance.
    /// </summary>
    public static Result<ServerInformation> FromParameter(
        string? host,
        int port,
        string? userId,
        string? password,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validHost = host
            .ToResultIsNotNullOrWhitespace(failureMessage: "Host cannot be empty.", nameof(host))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validPort = port
            .ToResult(nameof(port))
            .Check(value => value is >= 0 and <= 65535, failureMessage: "Port must be between 0 and 65535.",
                nameof(port))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validUserId = userId
            .ToResultIsNotNullOrWhitespace(nameof(userId))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validPassword = password
            .ToResultIsNotNullOrWhitespace(nameof(password))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<ServerInformation>.Success(
                new ServerInformation(validHost, validPort, validUserId, validPassword),
                resultId)
            : Result<ServerInformation>.Failure(
                failures.ToError(message: "Unable to create server information."),
                resultId);
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override bool EqualsCore(ServerInformation other)
    {
        return this.Host.Equals(other.Host, StringComparison.OrdinalIgnoreCase)
               && this.Port == other.Port
               && this.UserId.Equals(other.UserId, StringComparison.OrdinalIgnoreCase)
               && this.Password == other.Password;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override int GetHashCodeCore()
    {
        return (this.Host, this.Port, this.UserId, this.Password).GetHashCode();
    }
}
