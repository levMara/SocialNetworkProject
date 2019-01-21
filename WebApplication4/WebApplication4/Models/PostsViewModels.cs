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

        public string JsonMentions { get; set; }

        public PostPermission postPermission;
    }

    public enum PostPermission
    {
        All,
        followers
    }

    public class PostModel
    {
        public string Id { get; set; }
        public ICollection<string> Mentions { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
        public int Likes { get; set; }
        public PostPermission postPermission;
    }

}