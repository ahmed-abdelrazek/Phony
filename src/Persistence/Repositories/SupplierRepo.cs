using Phony.Kernel.Repositories;
using Phony.Model;

namespace Phony.Persistence.Repositories
{
    public class SupplierRepo : Repository<Supplier>, ISupplierRepo
    {
        public SupplierRepo(PhonyDbContext context) : base(context)
        {
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}