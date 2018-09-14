using DATA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE_P
{

    public abstract class Service<T> : IService<T> where T : class
    {
        IUnitOfWork utw;
        protected Service(IUnitOfWork utw)
        {
            this.utw = utw;
        }

        public virtual void Add(T entity)
        {
            ////_repository.Add(entity);
            utw.getRepository<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            //_repository.Update(entity);
            utw.getRepository<T>().Update(entity);
        }

        public virtual void Delete(T entity)
        {
            //   _repository.Delete(entity);
            utw.getRepository<T>().Delete(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            // _repository.Delete(where);
            utw.getRepository<T>().Delete(where);
        }

        public virtual T GetById(long id)
        {
            //  return _repository.GetById(id);
            return utw.getRepository<T>().GetById(id);
        }

        public virtual T GetById(string id)
        {
            //return _repository.GetById(id);
            return utw.getRepository<T>().GetById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return utw.getRepository<T>().GetAll();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> filter = null, Expression<Func<T, bool>> orderBy = null)
        {
            //  return _repository.GetAll();
            return utw.getRepository<T>().GetMany(filter, orderBy);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            //return _repository.Get(where);
            return utw.getRepository<T>().Get(where);
        }

        public void Commit()
        {
            try
            {
                utw.Commit();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void CommitAsync()
        {
            try
            {
                utw.CommitAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Dispose()
        {
            utw.Dispose();
        }
    }

}
