using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HinesSite.ViewModels
{
    public class BlogpostGroup
    {
        public int BlogpostId { get; set; }

        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedOn { get; set; }

        // public int BlogpostCount { get; set; }
    }
}