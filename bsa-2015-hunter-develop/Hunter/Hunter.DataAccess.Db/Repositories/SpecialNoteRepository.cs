using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class SpecialNoteRepository : Repository<SpecialNote>, ISpecialNoteRepository
    {
        public SpecialNoteRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}
