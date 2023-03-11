using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Application;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
   Task<Result<Unit>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
