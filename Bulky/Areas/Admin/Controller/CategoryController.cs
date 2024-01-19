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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bulky.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj) {

            if (obj.Name == obj.DisplayOrder.ToString()) {
                ModelState.AddModelError("Name", "Dislay order cannot be the same as name");
            }
            if (ModelState.IsValid) {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }


        public IActionResult Edit(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _unitOfWork.Category.Get(u=> u.Id == id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

         [HttpPost]
        public IActionResult Edit(Category obj) {

            if (ModelState.IsValid) {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category Edited successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }

         public IActionResult Delete(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _unitOfWork.Category.Get(u=> u.Id == id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            
            Category? obj = _unitOfWork.Category.Get(u=> u.Id == id);

            if (obj == null) {
                NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}