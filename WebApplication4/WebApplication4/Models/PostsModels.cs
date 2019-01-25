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

        public PostPermission PostPermission;
    }

    public class UploadCommentViewModel
    {
        public string PostId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string CommentContent { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Upload an Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [Display(Name = "Seen By")]
        public PostPermission PostPermission { get; set; }

        public string JsonMentions { get; set; }
    }


    public enum PostPermission
    {
        [Display(Name ="All")]
        all,
        [Display(Name ="My followers only")]
        followers
    }

    public class PostModel:CommentModel
    {
        public PostPermission Permission { get; set; }
    }

    public class PostWithCommentsModel:PostModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }
    }

    public class CommentModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public ICollection<string> Mentions { get; set; }
        public string ImageUrl { get; set; }
        public int Likes { get; set; }
    }



}