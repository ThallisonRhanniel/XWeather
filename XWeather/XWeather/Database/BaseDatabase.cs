using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using SQLite;
using Xamarin.Essentials;

namespace XWeather.Database
{
    public class BaseDatabase
    {
        private static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, $"{nameof(XWeather)}.db3");


        //A inicialização "LAZY" de um objeto significa que sua criação é adiada até que seja usado pela primeira vez.
        static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder =
            new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(_databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));

        static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;

        protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnection<T>()
        {
            if (DatabaseConnection.TableMappings.All(x => x.MappedType.Name != typeof(T).Name))
            {
                await DatabaseConnection.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
                await DatabaseConnection.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
            }

            return DatabaseConnection;
        }



        protected static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetryAsync(numRetries, PollyRetryAttempt).ExecuteAsync(action);

            TimeSpan PollyRetryAttempt(int attemptNumber)
            {
                return TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
            }
        }
    }
}
