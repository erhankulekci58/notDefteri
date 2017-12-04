using NotDefteri.BusinessLayer.Abstract;
using NotDefteri.BusinessLayer.Results;
using NotDefteri.Common.Helpers;
using NotDefteri.DataAccessLayer.EntityFreamwork;
using NotDefteri.Entities;
using NotDefteri.Entities.Messages;
using NotDefteri_AspNetMvc.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotDefteri.BusinessLayer
{
    public class UserManager : ManagerBase<ND_User>
    {

        public BusinessLayerResult<ND_User> RegisterUser(RegisterViewModels data)
        {
            ND_User user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<ND_User> layerResult = new BusinessLayerResult<ND_User>();
            

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessagesCode.UsernameAlreadyExist,"Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessagesCode.EmailAlreadyExist,"E-mail adresi kayıtlı.");
                }

            }
            else
            {
                int dbResult = base.Insert(new ND_User()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivatedGuid = Guid.NewGuid(),
                    ProfileImageFilename = "user-default.png",
                    IsActive = false,
                    IsAdmin = false

                });

                if (dbResult > 0)
                {
                    layerResult.Result = Find(x => x.Email == data.Email && x.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActivatedGuid}";
                    string body = $"Merhaba {layerResult.Result.Username} hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";

                    MailHelper.SendMail(body,layerResult.Result.Email,"Not Defteri Hesap Aktifleştirme");
                }
            }

            return layerResult;
        }

        public BusinessLayerResult<ND_User> GetUserById(int id)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<ND_User> LoginUser(LoginViewModels data)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();

            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessagesCode.UserIsNotActive,"Hesap aktif değildir.");
                    res.AddError(ErrorMessagesCode.CheckYourEmail, "Lütfen hesabınızı aktifleştirin.");
                }
            }
            else
            {
                res.AddError(ErrorMessagesCode.UsernameOrPassWrong,"Kullanıcı adı ya da parola hatalı.");
            }

            return res;
        }

        public BusinessLayerResult<ND_User> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();
            res.Result = Find(x => x.ActivatedGuid == activateId);

            if (res.Result != null )
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessagesCode.UserAlreadyActive, "Hesap zaten aktiftir.");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessagesCode.ActivateIdDoesNotExist, "Hesabınız aktifleştirilemedi.");
            }

            return res;
        }

        public BusinessLayerResult<ND_User> UpdateProfile(ND_User data)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();
            ND_User db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessagesCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessagesCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<ND_User> RemoveUserById(int id)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();
            ND_User user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessagesCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessagesCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        //method hiding
        /*(bir metodu ezmek ve geri dönüş tipini değiştirebilmek için bu yöntemi kullanıyoruz.) 
         override'da geri dönüş tipini değiştiremiyoruz.*/
        public new BusinessLayerResult<ND_User> Insert(ND_User data)
        {
            ND_User user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<ND_User> layerResult = new BusinessLayerResult<ND_User>();

            layerResult.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessagesCode.EmailAlreadyExist, "E-mail adresi kayıtlı.");
                }

            }
            else
            {
                layerResult.Result.ActivatedGuid = Guid.NewGuid();
                layerResult.Result.ProfileImageFilename = "user-default.png";

                if (base.Insert(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessagesCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return layerResult;
        }

        public new BusinessLayerResult<ND_User> Update(ND_User data)
        {
            BusinessLayerResult<ND_User> res = new BusinessLayerResult<ND_User>();
            ND_User db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));

            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessagesCode.EmailAlreadyExist, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessagesCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }
    }
}
