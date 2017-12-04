using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotDefteri.Entities;
using NotDefteri.BusinessLayer;
using NotDefteri_AspNetMvc.Models;
using NotDefteri_AspNetMvc.Filters;

namespace NotDefteri_AspNetMvc.Controllers
{
    [Exc]
    [AuthAdmin]
    [Auth]
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();
       
        public ActionResult Index()
        {
            return View(categoryManager.List());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Category nD_Category = categoryManager.Find(x => x.Id == id.Value);

            if (nD_Category == null)
            {
                return HttpNotFound();
            }
            return View(nD_Category);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ND_Category nD_Category)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                categoryManager.Insert(nD_Category);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }

            return View(nD_Category);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Category nD_Category = categoryManager.Find(x => x.Id == id.Value);

            if (nD_Category == null)
            {
                return HttpNotFound();
            }

            return View(nD_Category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ND_Category nD_Category)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                ND_Category category = categoryManager.Find(x => x.Id == nD_Category.Id);
                category.Title = nD_Category.Title;
                category.Description = nD_Category.Description;

                categoryManager.Update(category);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }
            return View(nD_Category);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_Category nD_Category = categoryManager.Find(x => x.Id == id.Value);
            if (nD_Category == null)
            {
                return HttpNotFound();
            }
            return View(nD_Category);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ND_Category nD_Category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(nD_Category);
            CacheHelper.RemoveCategoriesFromCache();
            
            return RedirectToAction("Index");
        }
        
    }
}
