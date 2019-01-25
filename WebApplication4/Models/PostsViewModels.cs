using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class UploadPostViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Post Content")]
        public string PostContent { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Upload an Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}