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
            Items = new ItemRepo(_context);
            Clients = new ClientRepo(_context);
            Companies = new CompanyRepo(_context);
            Suppliers = new SupplierRepo(_context);
        }

        public IUserRepo Users { get; private set; }
        public IItemRepo Items { get; private set; }
        public IClientRepo Clients { get; private set; }
        public ICompanyRepo Companies { get; private set; }
        public ISupplierRepo Suppliers { get; private set; }

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