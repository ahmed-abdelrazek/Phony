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

        bool IUserRepo.GetLoginCredentials(string Name, string Pass)
        {
            return PhonyDbContext.Users.Any(u => u.Name == Name && u.Pass == Pass && u.IsActive == true);
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}
