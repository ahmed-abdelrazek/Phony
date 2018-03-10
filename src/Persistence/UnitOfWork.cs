using Phony.Kernel;
using Phony.Kernel.Repositories;
using Phony.Persistence.Repositories;

namespace Phony.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PhonyDbContext _context;

        public UnitOfWork(PhonyDbContext context)
        {
            _context = context;
            Users = new UserRepo(_context);
        }

        public IUserRepo Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
