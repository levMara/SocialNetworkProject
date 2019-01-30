using SocialBL.Dal;
using SocialBL.Exceptiones;
using SocialBL.Models;
using SocialBL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Manageres
{
    public class PostManager
    {
        Neo4jPostService _Neo4jpostService;

        public PostManager()
        {
            _Neo4jpostService = new Neo4jPostService();
        }

        public int LikePost(string userId, string postId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(postId))
                throw new IncorrectDetailsException("User id or post id empty.");

           return _Neo4jpostService.LikePost(userId, postId);
        }

        public void AddPost(string userId, Post post)
        {
            if (userId == null || post == null)
                throw new IncorrectDetailsException("User or post empty");

            post = NewPostHandling(post);
            _Neo4jpostService.AddPost(userId, post);

            if (post.Mentions.Count > 0)
                _Neo4jpostService.AddPostMention(post.Id, post.Mentions);

        }

        private Post NewPostHandling(Post post)
        {
            post.Id = Guid.NewGuid().ToString();
            post.Likes = 0;
            post.Date = DateTime.UtcNow;

            return post;
        }

        public List<Post> GetMyPosts(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new IncorrectDetailsException("User id empty.");

            return _Neo4jpostService.GetMyPosts(userId);
        }


        public Post GetPost(string postId)
        {
            if (string.IsNullOrEmpty(postId))
                throw new IncorrectDetailsException("post id empty.");

            return _Neo4jpostService.GetPost(postId);
        }

        public string GetUploader(string postId)
        {
            if (string.IsNullOrEmpty(postId))
                throw new IncorrectDetailsException("post id empty.");

            return _Neo4jpostService.GetUploader(postId);
        }
    }
}
