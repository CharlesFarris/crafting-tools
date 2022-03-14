namespace CraftingTools.Shared;

public static class GuidExtensions
{
    public static RailwayResult<Guid> ToValidResult(
        this Guid guid,
        string? failureMessage = default,
        string? resultId = default)
    {
        return guid
            .ToResult(resultId)
            .Check(value => value != Guid.Empty, failureMessage ?? "Guid cannot be null.");
    }
}