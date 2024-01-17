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
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category {get; private set;}
        public IProductRepository Product {get; private set;}
        public ICompanyRepository Company {get; set;}
        public IShoppingCartRepository ShoppingCart {get; set;}
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public UnitOfWork(ApplicationDbContext db) {
            _db = db;
            Category = new CategoryRepository(_db); 
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }
        
        public void Save() {
            _db.SaveChanges();
        }
    }
}