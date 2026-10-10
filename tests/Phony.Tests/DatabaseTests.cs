using LiteDB;
using Phony.Data;
using Phony.DTOs;
using Phony.Mappers;
using Phony.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests
{
    public sealed class DatabaseTests : DatabaseTestBase
    {
        [Fact]
        public void BackupWithAnotherPasswordCannotReplaceTheWorkingDatabase()
        {
            var backup = Path.Combine(_folder, "wrong-password.bak");
            using (var db = new LiteDatabase(LiteDbContext.GetConnectionString(backup, "another password")))
            {
                db.GetCollection<User>(DBCollections.Users).Insert(_user);
            }

            Assert.ThrowsAny<Exception>(() => DatabaseBackup.Restore(_connection, backup));
            using var verify = new LiteDatabase(_connection);
            Assert.Equal(10, verify.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
        }

        [Fact]
        public void EncryptedDatabaseBackupAndRestoreUseTheConfiguredFilename()
        {
            var backup = Path.Combine(_folder, "backup.bak");
            DatabaseBackup.Create(_connection, backup);
            Save();
            DatabaseBackup.Restore(_connection, backup);
            using var db = new LiteDatabase(_connection);
            Assert.Equal(10, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
            Assert.Equal(0, db.GetCollection<Bill>(DBCollections.Bills).Count());
            Assert.Single(Directory.GetFiles(_folder, "*.before-restore-*.bak"));
        }

        [Fact]
        public void InvalidBackupDoesNotReplaceTheDatabase()
        {
            var backup = Path.Combine(_folder, "invalid.bak");
            File.WriteAllText(backup, "invalid database");
            Assert.ThrowsAny<Exception>(() => DatabaseBackup.Restore(_connection, backup));
            using var db = new LiteDatabase(_connection);
            Assert.Equal(10, db.GetCollection<Item>(DBCollections.Items).FindById(1).QTY);
        }

    }
}
