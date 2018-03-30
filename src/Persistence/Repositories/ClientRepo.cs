using Phony.Kernel.Repositories;
using Phony.Model;

namespace Phony.Persistence.Repositories
{
    public class ClientRepo : Repository<Client>, IClientRepo
    {
        public ClientRepo(PhonyDbContext context) : base(context)
        {
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}