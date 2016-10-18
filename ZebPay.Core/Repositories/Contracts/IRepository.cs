using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZebPay.Core.Repositories.Contracts
{
    public interface IRepository<T> where T:IDisposable
    {
        #region Methods
        bool Add(T entity);
        bool AddMultiple(List<T> entities);

        IQueryable<T> Find(Expression<Func<T, bool>> where);

        void Dispose();

        #endregion
    }
}
