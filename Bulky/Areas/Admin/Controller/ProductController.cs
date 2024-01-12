using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository;
using Bulky.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bulky.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }
        
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj) {

            if (ModelState.IsValid) {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index","Product");
            }
            return View();
        }


        public IActionResult Edit(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.Product.Get(u=> u.Id == id);
            if (productFromDb == null) {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj) {

            if (ModelState.IsValid) {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Product Edited successfully";
                return RedirectToAction("Index","Product");
            }
            return View();
        }

         public IActionResult Delete(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.Product.Get(u=> u.Id == id);
            if (productFromDb == null) {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            
            Product? obj = _unitOfWork.Product.Get(u=> u.Id == id);

            if (obj == null) {
                NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}