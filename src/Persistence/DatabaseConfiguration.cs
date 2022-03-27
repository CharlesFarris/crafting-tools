using System.Collections.Immutable;
using SleepingBearSystems.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystem.CraftingTools.Persistence;

public sealed class DatabaseConfiguration : ValueObject<DatabaseConfiguration>
{
    private DatabaseConfiguration(string database, string userId, string password)
    {
        this.Database = database;
        this.UserId = userId.ToMaybe();
        this.Password = password.ToMaybe();
    }

    public string Database { get; }

    public Maybe<string> UserId { get; }

    public Maybe<string> Password { get; }

    public static Result<DatabaseConfiguration> FromParameters(
        string? database,
        bool integratedSecurity,
        string? userId,
        string? password,
        string? resultId = default)
    {
        var failures = ImmutableList<ResultBase>.Empty;

        var validDatabase = database
            .ToResultIsNotNullOrWhitespace(nameof(database))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validUserId = userId
            .ToResultIsNotNullOrWhitespace(nameof(userId))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validPassword = password
            .ToResultIsNotNullOrWhitespace(nameof(password))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? Result<DatabaseConfiguration>.Success(
                new DatabaseConfiguration(validDatabase, validUserId, validPassword), resultId)
            : Result<DatabaseConfiguration>.Failure(
                failures.ToError(message: "Unable to create database configuration."), resultId);
    }

    protected override bool EqualsCore(DatabaseConfiguration other)
    {
        return true; //this.UserId == other.UserId && this.Password == other.Password;
    }

    protected override int GetHashCodeCore()
    {
        return (this.UserId, this.Password).GetHashCode();
    }
}
