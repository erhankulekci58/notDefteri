using NotDefteri.Common;
using NotDefteri.Entities;
using NotDefteri_AspNetMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotDefteri_AspNetMvc.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (CurrentSession.User != null)
            {
                ND_User user = CurrentSession.User as ND_User;
                return user.Username;
            }

            return "system";
        }
    }
}