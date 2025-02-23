using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore.Modules;

public class CommandInterceptors : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine($"Inside the {nameof(ReaderExecuting)} method");
        UseNoLock(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Inside the {nameof(ReaderExecutingAsync)} method");
        UseNoLock(command);
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static void UseNoLock(DbCommand command)
    {
        if (!command.CommandText.Contains("WITH (NOLOCK)"))
            command.CommandText = _table.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
    }

    private static readonly Regex _table = new(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
}

public class ConnectionInterceptor : DbConnectionInterceptor
{
    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        Console.WriteLine($"Inside the {nameof(ConnectionOpening)} method");

        var connectionString = ((SqlConnection)connection).ConnectionString;

        Console.WriteLine($"{nameof(connection.ConnectionString)}: {connectionString}");

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            ApplicationName = "DominandoEFCore"
        };

        connection.ConnectionString = connectionStringBuilder.ToString();

        Console.WriteLine(connection.ConnectionString);

        return base.ConnectionOpening(connection, eventData, result);
    }
}

public class PersistenceInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Console.WriteLine($"Inside the {nameof(SavingChanges)} method");

        Console.WriteLine(eventData.Context.ChangeTracker.DebugView.LongView);

        return base.SavingChanges(eventData, result);
    }
}