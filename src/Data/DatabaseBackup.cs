using LiteDB;
using System;
using System.IO;

namespace Phony.Data
{
    public static class DatabaseBackup
    {
        public static void Create(ConnectionString connection, string destination)
        {
            if (!File.Exists(connection.Filename))
            {
                throw new FileNotFoundException("Database not found.");
            }

            // Direct mode holds the writer lock while checkpointing and copying.
            using var db = new LiteDatabase(new ConnectionString
            {
                Filename = connection.Filename, Password = connection.Password, Connection = ConnectionType.Direct
            });
            db.Checkpoint();
            File.Copy(connection.Filename, destination, false);
        }

        public static void Restore(ConnectionString connection, string backup)
        {
            var target = Path.GetFullPath(connection.Filename);
            if (string.Equals(target, Path.GetFullPath(backup), StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Backup and database paths must differ.");
            }

            var staged = target + ".restore-" + Guid.NewGuid().ToString("N");
            try
            {
                File.Copy(backup, staged, false);
                // Validate the staged database before touching the working database.
                using (var db = new LiteDatabase(new ConnectionString { Filename = staged, Password = connection.Password }))
                {
                    if (db.GetCollection<Models.User>(DBCollections.Users).Count() == 0
                        || db.GetCollection<Models.Store>(DBCollections.Stores).FindById(1) == null
                        || db.GetCollection<Models.Treasury>(DBCollections.Treasuries).FindById(1) == null)
                        throw new InvalidOperationException("Backup is not a configured Phony database.");
                    foreach (var name in db.GetCollectionNames())
                        foreach (var document in db.GetCollection(name).FindAll()) { }
                    db.Checkpoint();
                }

                if (File.Exists(target))
                {
                    using (var db = new LiteDatabase(new ConnectionString
                    {
                        Filename = target, Password = connection.Password, Connection = ConnectionType.Direct
                    })) { db.Checkpoint(); }
                    // Preserve the previous database and replace atomically.
                    File.Replace(staged, target, target + ".before-restore-" + Guid.NewGuid().ToString("N") + ".bak");
                }
                else File.Move(staged, target);
            }
            finally
            {
                if (File.Exists(staged))
                {
                    File.Delete(staged);
                }
            }
        }
    }
}
