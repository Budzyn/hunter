using System.Data.Entity;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db.Base
{
    public interface IDatabaseFactory
    {
        DbContext Get();
    }

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext dataContext;

        public DbContext Get()
        {
            return dataContext ?? (dataContext = new HunterDbContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
