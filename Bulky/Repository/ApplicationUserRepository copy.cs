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
    public class ProductImageRepository : Repository<ProductImages>, IProductImageRepository {
        private ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProductImages productImages) {
            _db.ProductImages.Update(productImages);
        }
    }
}