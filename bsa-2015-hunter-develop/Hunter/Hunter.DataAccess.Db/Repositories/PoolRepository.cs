using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class PoolRepository : Repository<Pool>, IPoolRepository
    {
        public PoolRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}
