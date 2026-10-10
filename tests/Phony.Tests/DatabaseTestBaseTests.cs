using LiteDB;
using Phony.Data;
using Phony.Models;
using System;
using System.IO;
using Xunit;

namespace Phony.Tests;

public sealed class DatabaseTestBaseTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FixtureInitializesItsOwnDatabaseAndRestoresPreviousState(bool previouslyConfigured)
    {
        var original = LiteDbContext.ConfiguredConnectionString;
        var previous = previouslyConfigured
            ? LiteDbContext.GetConnectionString(Path.Combine(Path.GetTempPath(), "PhonyTests-unused.db"))
            : null;
        try
        {
            LiteDbContext.ConfiguredConnectionString = previous;
            string testDatabasePath;
            using (var fixture = new TestDatabase())
            {
                testDatabasePath = LiteDbContext.ConnectionString.Filename;
                Assert.NotSame(previous, LiteDbContext.ConnectionString);
                using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                Assert.Equal("test", db.GetCollection<User>(DBCollections.Users).FindById(1).Name);
            }

            Assert.Same(previous, LiteDbContext.ConfiguredConnectionString);
            Assert.False(File.Exists(testDatabasePath));
            if (!previouslyConfigured)
                Assert.Throws<InvalidOperationException>(() => LiteDbContext.ConnectionString);

            // A second fixture must work after the first restores an uninitialized context.
            using (var fixture = new TestDatabase())
                Assert.NotEqual(testDatabasePath, LiteDbContext.ConnectionString.Filename);
            Assert.Same(previous, LiteDbContext.ConfiguredConnectionString);
        }
        finally
        {
            LiteDbContext.ConfiguredConnectionString = original;
        }
    }

    private sealed class TestDatabase : DatabaseTestBase { }
}
