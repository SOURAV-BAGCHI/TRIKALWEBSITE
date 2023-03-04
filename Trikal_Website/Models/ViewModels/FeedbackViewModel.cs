using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trikal_Website.Models.ViewModels
{
    public class FeedbackViewModel
    {
        public Int32 ProjectId { get; set; }
        public Int64 RecordId { get; set; }
        public String Name { get; set; }
        public String UserImageUrl { get; set; }
        public String FeedbackImageUrl { get; set; }
        public List<String> FeedbackImageUrlList { get; set; }
        public Double Ratings { get; set; }
        public String FeedbackText { get; set; }
        public String CreateDate { get; set; }

    }
}