using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Data
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);
        void CommitTransaction();
        void ExecuteInTransaction(Action action);
        T ExecuteInTransaction<T>(Func<T> action);
        IDbContextTransaction GetCurrentTransaction();
        void RejectAllChanges();
        void RollbackTransaction();
        int SaveChanges();
    }
}
