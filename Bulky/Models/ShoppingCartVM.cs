using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}