using Phony.Kernel.Repositories;
using Phony.Model;

namespace Phony.Persistence.Repositories
{
    public class CompanyRepo : Repository<Company>, ICompanyRepo
    {
        public CompanyRepo(PhonyDbContext context) : base(context)
        {
        }

        public PhonyDbContext PhonyDbContext
        {
            get { return Context as PhonyDbContext; }
        }
    }
}