using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository;
using Bulky.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace Bulky.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviorment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) {
            _unitOfWork = unitOfWork;
            _webHostEnviorment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u=> new SelectListItem{
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(objProductList);
             
        }
        
        public IActionResult Create() {

            ProductVM productVM = new() {
            CategoryList =_unitOfWork.Category.GetAll().Select(u=> new SelectListItem{
                Text = u.Name,
                Value = u.Id.ToString()
            }), Product = new Product()
            };

            return View(productVM);
        }
        
       // [HttpPost]
        public IActionResult Create(ProductVM productVM) {

            if (ModelState.IsValid) {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index","Product");
            } else {
                productVM.CategoryList =_unitOfWork.Category.GetAll().Select(u=> new SelectListItem{
                Text = u.Name,
                Value = u.Id.ToString()
            });

            }
            return View(productVM);
        }
        
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VatList = _unitOfWork.Vat.GetAll().Select(u=> new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u=>u.Id==id, includeProperties:"ProductImages");
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files) {

            if (ModelState.IsValid) {

                if (productVM.Product.Id == 0) {
                    _unitOfWork.Product.Add(productVM.Product);
                } else {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();

                 string wwwRootPath = _webHostEnviorment.WebRootPath;

                if (files != null) {
                    foreach (IFormFile file in files) {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"image/products/product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath)) {
                            Directory.CreateDirectory(finalPath);
                        }
                       
                       using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create)) {
                            file.CopyTo(fileStream);
                        }

                        ProductImages productImages = new() {
                            ImageUrl = @"/" + productPath + @"/" + fileName,
                            ProductId = productVM.Product.Id
                        };

                        if (productVM.Product.ProductImages == null)
                        productVM.Product.ProductImages = new List<ProductImages>();

                        productVM.Product.ProductImages.Add(productImages);
                        
                        _unitOfWork.Product.Update(productVM.Product);
                        _unitOfWork.Save();
                    }
                }


                /* 
                // Adding One Product 
                //
                string wwwRootPath = _webHostEnviorment.WebRootPath;
                if (file != null) {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) {
                        var oldImage = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImage)) {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                        file.CopyTo(filestream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }*/

            
                TempData["Success"] = "Product Created";
                return RedirectToAction("Index");
            } else {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }; 
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

        //[HttpPost]
        public IActionResult Edit(Product obj) {

            if (ModelState.IsValid) {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Product Edited successfully";
                return RedirectToAction("Index","Product");
            }
            return View();
        }

         /*public IActionResult Delete(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.Product.Get(u=> u.Id == id);
            if (productFromDb == null) {
                return NotFound();
            }

            return View(productFromDb);
        }*/

        /*[HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            
            Product? obj = _unitOfWork.Product.Get(u=> u.Id == id);

            if (obj == null) {
                NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted successfully";

            return RedirectToAction("Index");
        }*/

         public IActionResult DeleteImage(int imageId) {
            var imageToBeDeleted = _unitOfWork.ProductImages.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null) {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl)) {
                    var oldImagePath =
                                   Path.Combine(_webHostEnviorment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImages.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region APICalls

        [HttpGet]
        public IActionResult GetAll() {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int id) {
            Product? obj = _unitOfWork.Product.Get(u=> u.Id == id);  
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();   

            return Json(new {success = true, message = "Delete Successful"});
        }

        #endregion
    }
}