using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trikal_Website.Models.ViewModels
{
    public class PaginationViewModel
    {
        public Int32 Offset { get; set; }
        public Int32 ResultSize { get; set; }
    }
}