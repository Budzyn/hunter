using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Interface
{
    public interface ITestRepository : IRepository<Test>
    {
    }
}
