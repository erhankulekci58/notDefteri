using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NotDefteri.Entities;
using NotDefteri_AspNetMvc.Models;
using NotDefteri.BusinessLayer;
using NotDefteri_AspNetMvc.Filters;

namespace NotDefteri_AspNetMvc.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private LikedManager likedManager = new LikedManager();

        [Auth]
        public ActionResult Index()
        {
            var nD_Note = noteManager.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(nD_Note.ToList());
        }

        [Auth]
        public ActionResult MyLikedNotes()
        {
            var nD_Note = likedManager.ListQueryable().Include("LikedUSer").Include("Note").Where(
                x => x.LikedUser.Id == CurrentSession.User.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View("Index",nD_Note.ToList());
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                List<int> likedNoteIds = likedManager.List(
                    x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Note.Id)).Select(
                    x => x.Note.Id).ToList();

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int noteid,bool liked)
        {
            int res = 0;

            ND_Liked like = likedManager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id);
            ND_Note note = noteManager.Find(x => x.Id == noteid);

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likedManager.Insert(new ND_Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = noteManager.Update(note);

                return Json(new { hasError = false, errorMessage = "", result = note.LikeCount });
            }


            return Json(new { hasError = true, errorMessage = "Beğenme işlemi hatalı.", result = note.LikeCount });
        }

        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Note note = noteManager.Find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_NotesPartial", note);
        }

        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_Note nD_Note = noteManager.Find(x => x.Id == id.Value);
            if (nD_Note == null)
            {
                return HttpNotFound();
            }
            return View(nD_Note);
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ND_Note nD_Note)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                nD_Note.Owner = CurrentSession.User;
                noteManager.Insert(nD_Note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", nD_Note.CategoryId);
            return View(nD_Note);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_Note nD_Note = noteManager.Find(x => x.Id == id.Value);
            if (nD_Note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", nD_Note.CategoryId);
            return View(nD_Note);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ND_Note nD_Note)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                ND_Note note = noteManager.Find(x => x.Id == nD_Note.Id);
                note.Text = nD_Note.Text;
                note.Title = nD_Note.Title;
                note.IsDraft = nD_Note.IsDraft;
                note.CategoryId = nD_Note.CategoryId;

                noteManager.Update(note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", nD_Note.CategoryId);
            return View(nD_Note);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ND_Note nD_Note = noteManager.Find(x => x.Id == id.Value);
            if (nD_Note == null)
            {
                return HttpNotFound();
            }
            return View(nD_Note);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ND_Note nD_Note = noteManager.Find(x => x.Id == id);
            noteManager.Delete(nD_Note);
            return RedirectToAction("Index");
        }

    }
}
