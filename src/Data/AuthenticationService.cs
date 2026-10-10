using LiteDB;
using Phony.Models;
using System.Linq;

namespace Phony.Data
{
    public static class AuthenticationService
    {
        public static User Authenticate(ConnectionString connection, string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || password == null)
            {
                return null;
            }

            using var db = new LiteDatabase(connection);
            var user = db.GetCollection<User>(DBCollections.Users).Find(x => x.Name == name && x.IsActive).FirstOrDefault();
            return user != null && SecurePasswordHasher.Verify(password, user.Pass) ? user : null;
        }
    }
}
