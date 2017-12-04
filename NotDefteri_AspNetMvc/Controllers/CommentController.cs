using NotDefteri.BusinessLayer;
using NotDefteri.BusinessLayer.Results;
using NotDefteri.Entities;
using NotDefteri_AspNetMvc.Filters;
using NotDefteri_AspNetMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotDefteri_AspNetMvc.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();
        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id.Value);

            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_CommentsPartial",note.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Comment comment = commentManager.Find(x => x.Id == id.Value);

            if (comment == null)
            {
                return HttpNotFound();
            }

            comment.Text = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            } 

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ND_Comment comment = commentManager.Find(x => x.Id == id.Value);

            if (comment == null)
            {
                return HttpNotFound();
            }

            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        [HttpPost]
        public ActionResult Create(ND_Comment comment, int? noteid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ND_Note note = noteManager.Find(x => x.Id == noteid.Value);

                if (note == null)
                {
                    return HttpNotFound();
                }

                comment.Note = note;
                comment.Owner = CurrentSession.User;

                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
 
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
    }
}