using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotDefteri_AspNetMvc.Entities.ValueObject
{
    public class RegisterViewModels
    {
        [DisplayName("Kullanıcı adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(25,ErrorMessage ="{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }

        [DisplayName("E-mail"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-mail adresi giriniz."), 
            StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Email { get; set; }

        [DisplayName("Parola"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            DataType(DataType.Password), 
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }

        [DisplayName("Parola"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez."), 
            DataType(DataType.Password), 
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı."),
            Compare("Password",ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }
    }
}