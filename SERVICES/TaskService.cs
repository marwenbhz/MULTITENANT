using DATA.Infrastructure;
using SERVICE_P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public class TaskService : Service<DOMAIN.Entities.Task>, IEntityService
    {

        private static IDatabaseFactory dbf = new DatabaseFactory();
        private static IUnitOfWork ut = new UnitOfWork(dbf);

        public TaskService() : base(ut)
        {

        }
    }
}
