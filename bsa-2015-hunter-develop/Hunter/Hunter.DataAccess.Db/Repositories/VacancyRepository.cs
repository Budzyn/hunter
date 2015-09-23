using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class VacancyRepository : Repository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
