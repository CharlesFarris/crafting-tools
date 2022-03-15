using System.Collections.Immutable;
using CraftingTools.Common;

namespace CraftingTools.Persistence;

/// <summary>
/// Container class for a database connection.
/// </summary>
public sealed class DatabaseInformation : ValueObject<DatabaseInformation>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    private DatabaseInformation(ServerInformation serverInformation, string databaseName)
    {
        this.ServerInformation = serverInformation;
        this.DatabaseName = databaseName;
    }

    public ServerInformation ServerInformation { get; }

    public string DatabaseName { get; }

    public static readonly DatabaseInformation None = new(ServerInformation.None, string.Empty);

    /// <summary>
    /// Factory method for creating a <see cref="DatabaseInformation"/> instance.
    /// </summary>
    public static RailwayResult<DatabaseInformation> FromParameters(
        ServerInformation serverInformation,
        string databaseName,
        string? resultId = default)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validServiceInformation = serverInformation
            .ToResultIsNotNull(failureMessage: "Server information cannot be null.", nameof(serverInformation))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validDatabaseName = databaseName
            .ToResultIsNotNullOrWhitespace(failureMessage: "Database name cannot be empty.", nameof(databaseName))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<DatabaseInformation>.Success(
                new DatabaseInformation(validServiceInformation, validDatabaseName), resultId)
            : RailwayResult<DatabaseInformation>.Failure("Unable to create database information.".ToError(), resultId);
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override bool EqualsCore(DatabaseInformation other)
    {
        return this.ServerInformation == other.ServerInformation
               && this.DatabaseName == other.DatabaseName;
    }

    /// <inheritdoc cref="ValueObject{T}"/>
    protected override int GetHashCodeCore()
    {
        return (this.ServerInformation, this.DatabaseName).GetHashCode();
    }
}
