using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Models;

namespace Bulky.Repository.IRepository
{
    public interface  ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}