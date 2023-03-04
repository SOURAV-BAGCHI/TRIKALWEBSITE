using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trikal_Website.Models.ViewModels
{
    public class BrochureRequestDetailsViewModel
    {
        public String BrochureRequestId { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public Boolean SendBit { get; set; }
        public Boolean MarkAsSpam { get; set; }
        public String CreateDate { get; set; }
        public String LastUpdateDate { get; set; }
        public String LastUpdateByAdminId { get; set; }

        public Int32 ListType { get; set; }
        public Int32 Success { get; set; }
        public String Message { get; set; }
    }
}