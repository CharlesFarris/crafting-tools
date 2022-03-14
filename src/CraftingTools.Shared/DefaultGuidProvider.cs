namespace CraftingTools.Shared;

public sealed class DefaultGuidProvider : IGuidProvider
{
    private DefaultGuidProvider()
    {
    }

    public Guid Next()
    {
        return Guid.NewGuid();
    }

    public static readonly DefaultGuidProvider Instance = new();
}