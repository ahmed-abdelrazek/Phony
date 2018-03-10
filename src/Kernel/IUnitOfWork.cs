using Phony.Kernel.Repositories;
using System;

namespace Phony.Kernel
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepo Users { get; }

        int Complete();
    }
}
