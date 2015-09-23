using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        public TestRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}
