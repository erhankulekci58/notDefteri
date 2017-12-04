using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotDefteri_AspNetMvc.ViewModels
{
    public class OkViewModel : NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem başarılı";
        }
    }
}