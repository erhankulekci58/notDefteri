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
using NotDefteri.BusinessLayer.Results;
using NotDefteri_AspNetMvc.ViewModels;
using NotDefteri_AspNetMvc.Filters;

namespace NotDefteri_AspNetMvc.Controllers
{
    [Exc]
    [AuthAdmin]
    [Auth]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        public ActionResult Index()
        {
            return View(userManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_User nD_User = userManager.Find(x => x.Id == id.Value);

            if (nD_User == null)
            {
                return HttpNotFound();
            }
            return View(nD_User);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ND_User nD_User)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<ND_User> res = userManager.Insert(nD_User);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(nD_User);

                }
                return RedirectToAction("Index");
            }

            return View(nD_User);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_User nD_User = userManager.Find(x => x.Id == id.Value);
            if (nD_User == null)
            {
                return HttpNotFound();
            }
            return View(nD_User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ND_User nD_User)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<ND_User> res = userManager.Update(nD_User);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(nD_User);
                }
                return RedirectToAction("Index");
            }
            return View(nD_User);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_User nD_User = userManager.Find(x => x.Id == id.Value);
            if (nD_User == null)
            {
                return HttpNotFound();
            }
            return View(nD_User);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ND_User nD_User = userManager.Find(x => x.Id == id);
            userManager.Delete(nD_User);
            return RedirectToAction("Index");
        }

    }
}
