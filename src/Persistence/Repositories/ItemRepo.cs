using Phony.Kernel.Repositories;
using Phony.Model;

namespace Phony.Persistence.Repositories
{
    public class ItemRepo : Repository<Item>, IItemRepo
    {
        public ItemRepo(PhonyDbContext context) : base(context)
        {
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}