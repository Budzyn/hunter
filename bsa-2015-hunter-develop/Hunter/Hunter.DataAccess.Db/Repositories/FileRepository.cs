using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Repositories;

namespace Hunter.DataAccess.Db.Repositories
{
    public class FileRepository : Repository<File>, IFileRepository
    {
        public FileRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
