using NotDefteri.BusinessLayer;
using NotDefteri.BusinessLayer.Results;
using NotDefteri.Entities;
using NotDefteri_AspNetMvc.Entities.ValueObject;
using NotDefteri_AspNetMvc.Filters;
using NotDefteri_AspNetMvc.Models;
using NotDefteri_AspNetMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NotDefteri_AspNetMvc.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private UserManager userManager = new UserManager();

        // GET: Home
        public ActionResult Index()
        {
            //if (TempData["model"] != null)
            //{
            //    return View(TempData["model"] as List<ND_Note>);
            //}
            
            //return View(nm.GetAllNotes().OrderByDescending(x => x.ModifiedOn ).ToList());
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            ND_Category cat = categoryManager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }


            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessLayerResult<ND_User> res = userManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModels errorModel = new ErrorViewModels()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorModel);
            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            BusinessLayerResult<ND_User> res = userManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModels errorModel = new ErrorViewModels()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorModel);
            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(ND_User model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                BusinessLayerResult<ND_User> res = userManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModels errorNotifyObj = new ErrorViewModels()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<ND_User>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            BusinessLayerResult<ND_User> res = userManager.RemoveUserById(CurrentSession.User.Id);
            

            if (res.Errors.Count > 0)
            {
                ErrorViewModels errorNotifyObj = new ErrorViewModels()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            CurrentSession.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<ND_User> res = userManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                CurrentSession.Set<ND_User>("login", res.Result);
                return RedirectToAction("Index");
            }

            

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModels model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<ND_User> res = userManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel okModel = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };

                okModel.Items.Add("Kayıt olma işlemi başarılı. Lütfen e-mail adresinize gönderilen linkten üyeliğinizi aktifleştirin.");

                return View("Ok",okModel);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<ND_User> res = userManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModels errorModel = new ErrorViewModels()
                {
                    Items = res.Errors
                };
                //TempData["errors"] = res.Errors;
                return View("Error",errorModel);
            }

            OkViewModel okModel = new OkViewModel()
            {
                Title = "Kayıt Başarılı",
                RedirectingUrl = "/Home/Login"
            };

            okModel.Items.Add("Aktivasyon işleminiz başarı ile gerçekleştirildi. Artık not işlemlerini yapabilirsiniz.");

            return View("Ok",okModel);
        }

        public ActionResult LogOut()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {

            return View();
        }
    }

}