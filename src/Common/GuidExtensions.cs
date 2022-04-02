using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Common;

public static class GuidExtensions
{
    public static Result<Guid> ToResultNotEmpty(
        this Guid uid,
        string? failureMessage = default,
        string? resultTag = default)
    {
        return uid
            .ToResult(resultTag)
            .Check(value => value != Guid.Empty, failureMessage ?? "Guid cannot be empty.");
    }
}
