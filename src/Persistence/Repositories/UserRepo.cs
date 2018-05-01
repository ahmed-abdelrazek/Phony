using Phony.Kernel;
using Phony.Kernel.Repositories;
using Phony.Model;
using System.Linq;

namespace Phony.Persistence.Repositories
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(PhonyDbContext context) : base(context)
        {
        }

        User IUserRepo.GetLoginCredentials(string Name, string Pass)
        {
            var User = PhonyDbContext.Users.FirstOrDefault(u => u.Name == Name && u.IsActive == true);
            if (User != null)
            {
                if (SecurePasswordHasher.Verify(Pass, User.Pass))
                {
                    return User;
                }
            }
            return null;
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}