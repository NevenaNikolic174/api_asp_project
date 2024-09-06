using MyProject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.UseCases
{
    public abstract class EfUseCase
    {
        private readonly MyDBContext _dbCon;

        protected EfUseCase(MyDBContext context)
        {
            _dbCon = context;
        }

        protected MyDBContext Context => _dbCon;
    }
}
