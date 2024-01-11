using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bulky.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bulky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }


        public IActionResult Edit(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

         [HttpPost]
        public IActionResult Edit(Category obj) {

            if (ModelState.IsValid) {
                _db.Categories.Update(obj);
                _db.SaveChanges();

                TempData["Success"] = "Category Edited successfully";
                return RedirectToAction("Index","Category");
            }
            return View();
        }

         public IActionResult Delete(int? id) {

            if (id == null || id == 0) {
                return NotFound();
            }

            Category categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null) {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            
            Category? obj = _db.Categories.Find(id);

            if (obj == null) {
                NotFound();
            }

            _db.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Category Deleted successfully";

            return RedirectToAction("Index");
        }
    }
}