using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository.IRepository;
using BulkyBook.DataAccess.Repository.IRepository;

namespace Bulky.Repository
{
    public class VatRepository : Repository<Vat>, IVatRepository
    {
        private ApplicationDbContext _db;

        public VatRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
        
        public void Update(Vat obj) {
            _db.Vat.Update(obj);
        }
    }
}