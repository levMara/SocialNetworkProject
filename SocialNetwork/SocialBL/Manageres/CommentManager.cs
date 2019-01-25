using SocialBL.Dal;
using SocialBL.Exceptiones;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Manageres
{
    public class CommentManager
    {
        Neo4jPostService _postService;

        public CommentManager()
        {
            _postService = new Neo4jPostService();
        }


        public void AddComment(string userId, string postId, Comment comment)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(postId) || comment == null)
                throw new IncorrectDetailsException("User id or post id or comment empty.");

            NewCommentHandling(comment);
            _postService.AddComment(userId, postId, comment);

            if (comment.Mentions.Count > 0)
                _postService.AddCommentMention(comment.Id, comment.Mentions);
        }

        private Comment NewCommentHandling(Comment comment)
        {
            comment.Id = Guid.NewGuid().ToString();
            comment.Likes = 0;
            comment.Date = DateTime.UtcNow;

            return comment;
        }

        public List<Comment> GetComments(string postId)
        {
            if (string.IsNullOrEmpty(postId))
                throw new IncorrectDetailsException("Post id empty.");

            return _postService.GetComments(postId);
        }

        public int LikeComment(string userId, string commentId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(commentId))
                throw new IncorrectDetailsException("User id or post id empty.");

           return _postService.LikeComment(userId, commentId);
        }
    }
}
