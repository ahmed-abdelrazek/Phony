using Phony.Kernel.Repositories;
using System;

namespace Phony.Kernel
{
    public interface IUnitOfWork : IDisposable
    {
        IBillRepo Bills { get; }
        IBillMoveRepo BillsMoves { get; }
        IClientRepo Clients { get; }
        ICompanyRepo Companies { get; }
        ICompanyMoveRepo CompaniesMoves { get; }
        IItemRepo Items { get; }
        INoteRepo Notes { get; }
        ISalesManRepo SalesMen { get; }
        ISalesManMoveRepo SalesMenMoves { get; }
        IServiceRepo Services { get; }
        IServiceMoveRepo ServicesMoves { get; }
        IStoreRepo Stores { get; }
        ISupplierRepo Suppliers { get; }
        ISupplierMoveRepo SuppliersMoves { get; }
        IUserRepo Users { get; }

        int Complete();
    }
}
