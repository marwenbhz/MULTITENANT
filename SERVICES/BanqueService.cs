using DATA.Infrastructure;
using DOMAIN.Entities;
using SERVICE_P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public class BanqueService : Service<Banque>, IBanqueService
    {


        private static IDatabaseFactory dbf = new DatabaseFactory();
        private static IUnitOfWork ut = new UnitOfWork(dbf);

        public BanqueService()  : base(ut)
        { }

    }
}
