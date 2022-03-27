using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Common;

public static class GuidExtensions
{
    public static Result<Guid> ToResultNotEmpty(
        this Guid uid,
        string? failureMessage = default,
        string? resultId = default)
    {
        return uid
            .ToResult(resultId)
            .Check(value => value != Guid.Empty, failureMessage ?? "Guid cannot be empty.");
    }
}
