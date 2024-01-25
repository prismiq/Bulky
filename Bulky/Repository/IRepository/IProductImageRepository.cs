using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Models;
using Bulky.Repository.IRepository;

namespace Bulky.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImages>
    {
        void Update(ProductImages obj);
    }
}