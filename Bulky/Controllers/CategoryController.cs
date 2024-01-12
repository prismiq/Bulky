using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Bulky.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bulky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db) {
            _categoryRepo = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
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
                _categoryRepo.Add(obj);
                _categoryRepo.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }


        public IActionResult Edit(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _categoryRepo.Get(u=> u.Id == id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

         [HttpPost]
        public IActionResult Edit(Category obj) {

            if (ModelState.IsValid) {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();

                TempData["Success"] = "Category Edited successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }

         public IActionResult Delete(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _categoryRepo.Get(u=> u.Id == id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            
            Category? obj = _categoryRepo.Get(u=> u.Id == id);

            if (obj == null) {
                NotFound();
            }

            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["Success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}