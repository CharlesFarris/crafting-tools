using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Domain;
using SleepingBearSystems.Tools.Common;

namespace SleepingBearSystems.CraftingTools.Application;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task<Maybe<TEntity>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<ImmutableList<TEntity>> GetAll(CancellationToken cancellationToken = default);
}
