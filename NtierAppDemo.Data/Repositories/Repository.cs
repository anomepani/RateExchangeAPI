using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NtierAppDemo.Core.Entities;
using NtierAppDemo.Core.Entities.Contracts;
using NtierAppDemo.Core.Repositories.Contracts;

namespace NtierAppDemo.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        #region Member Variables

        protected DbSet<T> dbSet;
        protected DbContext dbContext;

        internal TransactionTypes transactionType = TransactionTypes.DbTransaction;
        internal TransactionScope transactionScope;
        internal System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadUncommitted;
        internal DbTransaction transaction;
        internal DbConnection connection;

        internal bool rethrowExceptions = true;
        internal bool useTransaction = false;
        internal int commandTimeout = 300;
        internal bool saveLastExcecutedMethodInfo = false;

        internal string connectionString = string.Empty;

        internal MethodBase lastExecutedMethod = null;

        public event RepositoryBaseExceptionHandler RepositoryBaseExceptionRaised;
        public delegate void RepositoryBaseExceptionHandler(Exception exception);

        public event RepositoryBaseRollBackOccuredHandler RepositoryBaseRollBackRaised;
        public delegate void RepositoryBaseRollBackOccuredHandler(MethodBase lastExecutedMethod);

        #endregion Member Variables

        #region Constructors

        protected Repository(DbContext dataContext)
        {
            dbContext = dataContext;
            dbSet = dbContext.Set<T>();
        }

        #endregion Constructors

        #region Methods

        public bool Add(T entity)
        {
            var result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    SetEntity().Add(entity);
                    SaveChanges();
                    result = true;
                }

                catch (Exception error)
                {
                    var entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (rethrowExceptions)
                    {
                        throw;
                    }

                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            OnRepositoryBaseExceptionRaised(error);
                        }
                    }
                }
            });

            return result;
        }

        public bool AddMultiple(List<T> entities)
        {
            var result = false;
            entities.ForEach(e => result = Add(e));
            return result;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> where)
        {
            var entities = default(IQueryable<T>);

            ProcessTransactionableMethod(() =>
            {
                entities = SetEntities().Where(where);
            });

            return entities;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var entities = SetEntities();

            ProcessTransactionableMethod(() =>
            {
                if (includes != null)
                {
                    entities = ApplyIncludesToQuery(entities, includes);
                }

                entities = entities.Where(where);
            });

            return entities;
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            var entry = dbContext.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                objectContext.Detach(entity);
            }
        }

        public void Detach(List<object> entities)
        {
            entities.ForEach(e => Detach(e));
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        internal DbSet<T> SetEntity()
        {
            var entity = dbContext.Set<T>();
            return entity;
        }

        internal IQueryable<T> SetEntities()
        {
            IQueryable<T> entities = dbContext.Set<T>();

            return entities;
        }

        internal DbEntityEntry SetEntry(T entity)
        {
            DbEntityEntry entry = dbContext.Entry(entity);
            return entry;
        }

        internal IQueryable<T> ApplyIncludesToQuery(IQueryable<T> entities, Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                entities = includes.Aggregate(entities, (current, include) => current.Include(include));
            }

            return entities;
        }

        internal void StartTransaction()
        {
            if (useTransaction)
            {
                switch (transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (transaction == null || transaction.Connection == null)
                            transaction = connection.BeginTransaction(isolationLevel);
                        break;

                    case TransactionTypes.TransactionScope:
                        transactionScope = new TransactionScope();
                        break;
                }
            }
        }

        public void CommitTransaction(bool startNewTransaction = false)
        {
            if (useTransaction)
            {
                switch (transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (transaction?.Connection != null)
                        {
                            SaveChanges();
                            transaction.Commit();
                        }
                        break;

                    case TransactionTypes.TransactionScope:
                        try
                        {
                            transactionScope?.Complete();
                        }
                        catch (Exception error)
                        {
                            if (rethrowExceptions)
                            {
                                throw;
                            }
                            else
                            {
                                if (RepositoryBaseExceptionRaised != null)
                                {
                                    RepositoryBaseExceptionRaised(error);
                                }
                            }
                        }

                        break;
                }

                if (startNewTransaction)
                    StartTransaction();
            }
            else
            {
                SaveChanges();
            }
        }

        internal void RollBack()
        {
            if (useTransaction)
            {
                if (transactionType == TransactionTypes.DbTransaction)
                {
                    if (transaction != null && transaction.Connection != null)
                    {
                        transaction.Rollback();

                        if (RepositoryBaseRollBackRaised != null)
                        {
                            RepositoryBaseRollBackRaised(lastExecutedMethod);
                        }
                    }
                }
            }
        }

        internal void ProcessTransactionableMethod(Action action)
        {
            if (saveLastExcecutedMethodInfo)
                lastExecutedMethod = MethodInfo.GetCurrentMethod();

            StartTransaction();
            action?.Invoke();
        }

        protected void OnRepositoryBaseExceptionRaised(Exception e)
        {
            var handler = RepositoryBaseExceptionRaised;

            if (handler != null)
            {
                handler(e);
            }
        }

        public void Dispose()
        {
            CommitTransaction();

            transaction = null;
            transactionScope = null;

            if (dbContext.Database.Connection != null && dbContext.Database.Connection.State != ConnectionState.Closed)
            {
                dbContext.Database.Connection.Close();
                dbContext.Database.Connection.Dispose();
            }

            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }

        #endregion Methods
    }
}
