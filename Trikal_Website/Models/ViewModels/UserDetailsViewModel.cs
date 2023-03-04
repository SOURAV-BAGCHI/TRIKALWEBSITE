using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trikal_Website.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public String Username { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String UserId { get; set; }
        public Boolean Success { get; set; }
        public String AdminId { get; set; }
        public String EmailOrPhone { get; set; }
    }
}