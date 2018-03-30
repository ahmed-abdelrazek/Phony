using Phony.Kernel.Repositories;
using System;

namespace Phony.Kernel
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepo Users { get; }
        IItemRepo Items { get; }
        IClientRepo Clients { get; }
        ICompanyRepo Companies { get; }
        ISupplierRepo Suppliers { get; }

        int Complete();
    }
}
