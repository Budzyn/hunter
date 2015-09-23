using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class ResumeRepository : Repository<Resume>, IResumeRepository
    {
        public ResumeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}
