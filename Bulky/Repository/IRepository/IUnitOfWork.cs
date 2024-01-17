using Bulky.Repository.IRepository;
using BulkyBook.DataAccess.Repository.IRepository;

namespace Bulky.Repository.IRepository {
    public interface IUnitOfWork {
        ICategoryRepository Category {get;}
        IProductRepository Product {get;}
        ICompanyRepository Company {get;}
        IShoppingCartRepository ShoppingCart {get;}
        IApplicationUserRepository ApplicationUser {get;}

        void Save();
    }
}