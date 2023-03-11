namespace SleepingBearSystems.CraftingTools.Domain;

public sealed record Item(Guid Id, string Name) : IEntity;

public sealed record CreateItemCommand(Guid Id, string Name) : ICommand;

public sealed record RenameItemCommand(Guid Id, string Name) : ICommand;

public sealed record ItemCreatedEvent(Guid Id, string Name) : IEvent;

public sealed record ItemRenamedEvent(Guid Id, string Name) : IEvent;

