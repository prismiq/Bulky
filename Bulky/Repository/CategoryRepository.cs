using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository.IRepository;

namespace Bulky.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
        
        public void Update(Category obj) {
            _db.Categories.Update(obj);
        }
    }
}