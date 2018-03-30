using Phony.Model;

namespace Phony.Kernel.Repositories
{
    public interface IUserRepo : IRepository<User>
    {
        User GetLoginCredentials(string Name, string Pass);
    }
}