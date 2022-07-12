using System.Collections.Immutable;
using NUnit.Framework;
using SleepingBearSystems.Tools.Common;
using SleepingBearSystems.Tools.Railway;
using ILogger = Serilog.ILogger;

namespace SleepingBearSystems.CraftingTools.Simulator.Test;

internal static class EngineTests
{
    [Test]
    public static void RunSimulation_ValidatesBehavior()
    {
        static void LogRunSimulation(ILogger localLogger, ImmutableList<Rotation> localRotations)
        {
            localLogger.Information("Count: {Count}", localRotations.Count);
            foreach (var rotation in localRotations)
            {
                localLogger.Information(
                    "Quality: {Quality}:  Steps: {Count} {Actions}",
                    rotation.Quality,
                    rotation.Actions.Count,
                    string.Join(",", rotation.Actions.Select(a => a.Name)));
            }
        }

        var list = new List<string>();
        var logger = list.CreateInMemoryLogger(timeStampFormat: string.Empty);
        var indentationMap = IndentationMap.Create();

        var player = new Player()
        {
            Level = 10,
            Craftsmanship = 100,
            Control = 100,
            CraftingPoints = 100
        };
        var recipe = new Recipe()
        {
            Level = 5,
            Durability = 10,
            Progress = 5,
            Quality = 5,
        };
        Engine
            .RunSimulation(player, recipe, r=> r.Quality == recipe.Quality, resultTag: "test_1")
            .LogResult(logger, indentationMap, LogRunSimulation);

        CollectionAssert.AreEqual(
            new[]
            {
                ""
            },
            list,
            string.Join(Environment.NewLine, list));
    }
}
