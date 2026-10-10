using LiteDB;
using Phony.Data;
using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests
{
    public sealed class LoginTests : DatabaseTestBase
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("test", true)]
        public void LoginRequiresUserName(string name, bool enabled)
        {
            UiThread.Run(() =>
            {
                var settings = Phony.Properties.Settings.Default;
                var configured = settings.IsConfigured;
                try
                {
                    settings.IsConfigured = false;
                    var vm = new LoginViewModel { Name = name };
                    Assert.Equal(enabled, vm.LogIn.CanExecute(null));
                }
                finally { settings.IsConfigured = configured; }
            });
        }

        [Fact]
        public void AuthenticationRejectsInactiveUsersAndWrongPasswords()
        {
            Assert.NotNull(AuthenticationService.Authenticate(_connection, "test", "password"));
            Assert.Null(AuthenticationService.Authenticate(_connection, "test", "wrong"));
            using (var db = new LiteDatabase(_connection))
            {
                _user.IsActive = false;
                db.GetCollection<User>(DBCollections.Users).Update(_user);
            }
            Assert.Null(AuthenticationService.Authenticate(_connection, "test", "password"));
        }

    }
}
