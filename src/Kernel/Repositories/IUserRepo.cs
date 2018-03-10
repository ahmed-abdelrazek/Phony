using Phony.Model;

namespace Phony.Kernel.Repositories
{
    public interface IUserRepo : IRepository<User>
    {
        bool GetLoginCredentials(string Name, string Pass);
    }
}
