using Bulky.Models;
using Bulky.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Repository.IRepository
{
    public interface IVatRepository : IRepository<Vat>
    {
        public void Update(Vat applicationUser);
    }
}
