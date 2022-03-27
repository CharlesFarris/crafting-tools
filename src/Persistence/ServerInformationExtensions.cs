using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Persistence;

public static class ServerInformationExtensions
{
    public static Result<ServerInformation> ParseConnectionString(string connectionString,
        string? resultId = default)
    {
        return connectionString
            .ToResultIsNotNullOrWhitespace(failureMessage: "Connection string cannot be empty.",
                nameof(connectionString))
            .OnSuccess(value =>
            {
                const string serverToken = "server=";
                const string portToken = "port=";
                const string userIdToken = "userId=";
                const string passwordToken = "password=";

                var tokens = value.Split(separator: ';', StringSplitOptions.RemoveEmptyEntries);
                var server = default(string);
                var port = -1;
                var userId = default(string);
                var password = default(string);
                foreach (var token in tokens)
                {
                    if (token.StartsWith(serverToken, StringComparison.OrdinalIgnoreCase))
                    {
                        server = token[..serverToken.Length];
                    }
                    else if (token.StartsWith(portToken, StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(token[..portToken.Length], out var p))
                        {
                            port = p;
                        }
                    }
                    else if (token.StartsWith(userIdToken, StringComparison.OrdinalIgnoreCase))
                    {
                        userId = token[..userIdToken.Length];
                    }
                    else if (token.StartsWith(passwordToken, StringComparison.OrdinalIgnoreCase))
                    {
                        password = token[..passwordToken.Length];
                    }
                }

                return ServerInformation.FromParameter(server, port, userId, password, resultId);
            });
    }
}
