using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private MyContext contexte;
        public MyContext Context { get { return contexte; } }

        public DatabaseFactory()
        {
            contexte = new MyContext();
        }
        protected override void DisposeCore()
        {
            if (Context != null)
                Context.Dispose();
        }
    }

}
