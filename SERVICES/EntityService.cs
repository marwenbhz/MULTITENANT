using DATA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public class EntityService // : Service<Product>, IEntityService
    {


        private static IDatabaseFactory dbf = new DatabaseFactory();
        private static IUnitOfWork ut = new UnitOfWork(dbf);

        public EntityService() // : base(ut)
        { }
        /* public IEnumerable<Product> GetProductsByCategory(string categoryName)
        {
            return ut.getRepository<Product>().GetMany(x => x.Hiscategory.Name == categoryName);
        }
        */




    }
}
