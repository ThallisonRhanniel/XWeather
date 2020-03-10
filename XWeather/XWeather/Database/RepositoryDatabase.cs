using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace XWeather.Database
{
    public class RepositoryDatabase : BaseDatabase
    {
        public enum ActionDb
        {
            Insert = 0,
            Update = 1,
            Erro = 2
        }

        public async Task<List<T>> GetAllItems<T>() where T : new()
        {
            var databaseConnection = await GetDatabaseConnection<T>().ConfigureAwait(false);

            return await AttemptAndRetry(() => databaseConnection.Table<T>().ToListAsync()).ConfigureAwait(false);
        }

        public async Task<int> SaveItem<T>(T item) where T : new()
        {
            var databaseConnection = await GetDatabaseConnection<T>().ConfigureAwait(false);

            switch (GetObjectId(item))
            {
                case (int)ActionDb.Insert:
                    return await AttemptAndRetry(() => databaseConnection.InsertAsync(item)).ConfigureAwait(false);
                case (int)ActionDb.Update:
                    return await AttemptAndRetry(() => databaseConnection.UpdateAsync(item)).ConfigureAwait(false);
                default:
                    return (int)ActionDb.Erro;
            }
        }


        public async Task<int> SaveList<T>(List<T> item, ActionDb actionDb) where T : new()
        {
            var databaseConnection = await GetDatabaseConnection<T>().ConfigureAwait(false);

            switch (actionDb)
            {
                case ActionDb.Insert:
                    return await AttemptAndRetry(() => databaseConnection.InsertAllAsync(item)).ConfigureAwait(false);
                case ActionDb.Update:
                    return await AttemptAndRetry(() => databaseConnection.UpdateAllAsync(item)).ConfigureAwait(false);
                default:
                    return (int)ActionDb.Erro;
            }
        }



        public async Task DeleteItem<T>(T item) where T : new()
        {
            var databaseConnection = await GetDatabaseConnection<T>().ConfigureAwait(false);
            await databaseConnection.DeleteAsync(item);
        }

        public async Task DeleteAllItens<T>() where T : new()
        {
            var databaseConnection = await GetDatabaseConnection<T>().ConfigureAwait(false);
            await databaseConnection.DeleteAllAsync<T>();
        }


        private int GetObjectId(object item)
        {

            int itemAction = (int)ActionDb.Insert;

            Type myType = item.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "Id")
                    itemAction = (int)prop.GetValue(item, null) == 0 ? (int)ActionDb.Insert : (int)ActionDb.Update;
            }

            return itemAction;
        }

    }
}
