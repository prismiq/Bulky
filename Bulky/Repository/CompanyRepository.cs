using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository.IRepository;

namespace Bulky.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
        
        public void Update(Company obj) {
            _db.Company.Update(obj);
        }
    }
}