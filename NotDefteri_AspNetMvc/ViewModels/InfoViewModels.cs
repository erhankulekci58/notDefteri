using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotDefteri_AspNetMvc.ViewModels
{
    public class InfoViewModels : NotifyViewModelBase<string>
    {
        public InfoViewModels()
        {
            Title = "Bilgilendirme";
        }
    }
}