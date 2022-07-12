using System.Collections.Immutable;
using SleepingBearSystems.CraftingTools.Simulator.Actions;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Simulator;

public static class Engine
{
    public static Result<ImmutableList<Rotation>> RunSimulation(
        Player player,
        Recipe recipe,
        Func<Rotation, bool>? rotationFilter = default,
        string? resultTag = default)
    {
        var job = new Job()
        {
            Player = player,
            Recipe = recipe
        };
        var jobState = new JobState()
        {
            Step = 1,
            CraftingPoints = player.CraftingPoints,
            Durability = recipe.Durability,
            Progress = 0,
            Quality = 0,
            Actions = ImmutableList<JobAction>.Empty
        };
        var availableActions = Engine.Actions
            .Where(a => a.Level <= player.Level)
            .ToImmutableList();
        var stack = new Stack<JobStep>();
        var successfulRotations = ImmutableList<Rotation>.Empty;
        foreach (var action in availableActions)
        {
            if (action.SuccessRate(job, jobState) < 100)
            {
                continue;
            }

            stack.Push(new JobStep()
            {
                JobAction = action,
                JobState = jobState
            });
        }

        while (stack.Count > 0)
        {
            var step = stack.Pop();
            var newJobState = step.JobAction.Execute(job, step.JobState);
            if (newJobState.Progress == job.Recipe.Progress)
            {
                successfulRotations = successfulRotations.Add(new Rotation()
                {
                    Quality = newJobState.Quality,
                    Actions = newJobState.Actions
                });
                continue;
            }

            if (newJobState.Durability <= 0)
            {
                continue;
            }

            foreach (var action in availableActions)
            {
                if (action.SuccessRate(job, jobState) < 100)
                {
                    continue;
                }

                stack.Push(new JobStep()
                {
                    JobAction = action,
                    JobState = newJobState
                });
            }
        }

        var filteredRotations = rotationFilter is null
            ? successfulRotations
            : successfulRotations.Where(rotationFilter).ToImmutableList();

        return Result<ImmutableList<Rotation>>.Success(filteredRotations, resultTag);
    }


    public static ImmutableList<JobAction> Actions { get; } = ImmutableList<JobAction>
        .Empty
        .Add(new BasicTouch())
        .Add(new BasicSynthesis())
        .Add(new MastersMend())
        .Add(new Observe())
        .Add(new WasteNot())
        .Add(new Veneration())
        .Add(new StandardTouch());
}
