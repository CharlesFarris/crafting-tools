﻿using System.Collections.Immutable;
using CraftingTools.Shared;

namespace CraftingTools.Domain;

public class Profession : Entity
{
    private Profession(Guid id, ProfessionName name) : base(id)
    {
        this.Name = name;
    }

    public static readonly Profession None = new(Guid.Empty, ProfessionName.None);
    public ProfessionName Name { get; }

    public static RailwayResult<Profession> FromParameters(Guid id, ProfessionName name, string? resultId = default)
    {
        var failures = ImmutableList<RailwayResultBase>.Empty;

        var validId = id
            .ToValidResult(failureMessage: "Id cannot be empty.", nameof(id))
            .UnwrapOrAddToFailuresImmutable(ref failures);

        var validName = name
            .ToResultIsNotNull(failureMessage: "Name cannot be null", nameof(name))
            .Check(value => value != ProfessionName.None, failureMessage: "Name cannot be none.")
            .UnwrapOrAddToFailuresImmutable(ref failures);

        return failures.IsEmpty
            ? RailwayResult<Profession>.Success(new Profession(validId, validName), resultId)
            : RailwayResult<Profession>.Failure(failures.ToError(message: "Unable to create profession."), resultId);
    }
}