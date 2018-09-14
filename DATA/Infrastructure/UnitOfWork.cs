using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private MyContext contexte;

        IDatabaseFactory dbFactory;
        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            contexte = dbFactory.Context;
        }

        public void Commit()
        {
            contexte.SaveChanges();
        }
        public void CommitAsync()
        {
            contexte.SaveChangesAsync();
        }
        public void Dispose()
        {
            contexte.Dispose();
        }
        public IRepositoryBase<T> getRepository<T>() where T : class
        {
            IRepositoryBase<T> repo = new RepositoryBase<T>(dbFactory);
            return repo;
        }

    }
}
