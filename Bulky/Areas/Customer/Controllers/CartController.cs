using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bulky.Models;
using Bulky.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bulky.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM {get; set;}
        public CartController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new() {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==userId,includeProperties:"Product")
            };

            foreach(var cart in ShoppingCartVM.ShoppingCartList) {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary() {
            return View();
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart) {
            if (shoppingCart.Count <= 50) {
                return shoppingCart.Product.Price;
            } else {
                if (shoppingCart.Count <= 100) {
                    return shoppingCart.Product.Price50;
                } else {
                    return shoppingCart.Product.Price100;
                }
            }
        }

        public IActionResult Plus(int cartId) {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id==cartId);
            cartFromDb.Count +=1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId) {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id==cartId);
            if (cartFromDb.Count <= 1) {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            } else {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId) {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u =>u.Id==cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}