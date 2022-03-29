using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Common;

public static class MaybeExtensions
{
    public static TValue? GetValueOrDefault<TValue>(this Maybe<TValue> maybe, TValue? defaultValue = default) where TValue : class
    {
        return maybe.HasValue
            ? maybe.Unwrap()
            : defaultValue;
    }
}
