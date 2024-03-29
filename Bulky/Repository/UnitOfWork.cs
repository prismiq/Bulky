using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using NuGet.Protocol.Plugins;

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
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IVatRepository Vat {get; private set;}
        public IProductImageRepository ProductImages {get; private set;}

        public UnitOfWork(ApplicationDbContext db) {
            _db = db;
            Category = new CategoryRepository(_db); 
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            Vat = new VatRepository(_db);
            ProductImages = new ProductImageRepository(_db);
        }
        
        public void Save() {
            _db.SaveChanges();
        }
    }
}