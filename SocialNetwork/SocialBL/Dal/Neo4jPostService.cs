using Neo4jClient;
using SocialBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Dal
{
    class Neo4jPostService
    {
        GraphClient _client;
        string _uri = "http://ec2-18-221-16-181.us-east-2.compute.amazonaws.com:7474/db/data";
        string _user = "neo4j";
        string _pass = "lm770";

        public Neo4jPostService()
        {
            _client = new GraphClient(new Uri(_uri), _user, _pass);
            _client.Connect();
        }




        #region post
        internal void AddPost(string userId ,Post post)
        {
            //func 1 or 2
            CreatePost(post);
            AddWriteRelation(userId ,post);
        }

        private void CreatePost(Post newPost)
        {
            _client.Cypher
                .Create("(p:Post {Id: {id}})")
                .Set("p = {newPost}")
                .WithParams(new
                {
                    id = newPost.Id,
                    newPost
                })
                .ExecuteWithoutResults();
        }

        private void AddWriteRelation(string userId, Post post)
        {
            _client.Cypher
                .Match("(u:User)", "(p:Post)")
                .Where((User u) => u.Id == userId)
                .AndWhere((Post p) => p.Id == post.Id)
                .CreateUnique("(u)-[:Write]->(p)")
                .ExecuteWithoutResults();
        }


        internal int LikePost(string userId, string postId)
        {
           var data = _client.Cypher
                .Match("(u:User)", "(p:Post)")
                .Where((User u) => u.Id == userId)
                .AndWhere((Post p) => p.Id == postId)
                .Merge("(u)-[:Like]->(p)")
                .OnCreate()
                .Set("p.Likes = p.Likes + 1")
                .Return((p) => p.As<Post>())
                .Results.FirstOrDefault();

            return data.Likes;
        }

        internal Post GetPost(string postId)
        {
            var query = _client.Cypher
                .Match("(p:Post)")
                .Where((Post p) => p.Id == postId)
                .Return((p) => p.As<Post>())
                .Results.ToList();

            return query.FirstOrDefault();
        }

        internal string GetUploader(string postId)
        {
            var query = _client.Cypher
                 .Match("(u:User)-[:Write]->(p:Post)")
                 .Where((Post p) => p.Id == postId)
                 .Return((u) => u.As<User>())
                 .Results.ToList();

            return query.FirstOrDefault().Id;
        }

        internal List<Post> GetMyPosts(string userId)
        {
            var query = _client.Cypher
                .Match("(u:User)-[:Write]->(p:Post)")
                .Where((User u) => u.Id == userId)
                .Return((p) => p.As<Post>())
                .Results.ToList();

            return query;
        }

        internal void AddPostMention(string postId ,ICollection<string> userIds)
        {
            foreach (var id in userIds)
            {
                _client.Cypher
                    .Match("(p:Post)", "(u:User)")
                    .Where((Post p) => p.Id == postId)
                    .AndWhere((User u) => u.Id == id)
                    .CreateUnique("(p)-[:Mention]->(u)")
                    .ExecuteWithoutResults();
            }
        }
        #endregion



        #region comment

        internal void AddComment(string userId, string postId, Comment newComment)
        {
            CreateComment(newComment);
            AddCommentedRelation(userId, newComment.Id);
            AddHas_commentRelation(postId, newComment.Id);
        }

        private void CreateComment(Comment newComment)
        {
            _client.Cypher
                .Create("(c:Comment {Id: {id}})")
                .Set("c = {newComment}")
                .WithParams(new
                {
                    id = newComment.Id,
                    newComment
                })
                .ExecuteWithoutResults();
        }

        private void AddCommentedRelation(string userId, string commentId)
        {
            _client.Cypher
                .Match("(u:User)", "(c:Comment)")
                .Where((User u) => u.Id == userId)
                .AndWhere((Comment c) =>c.Id == commentId)
                .CreateUnique("(u)-[:Commented]->(c)")
                .ExecuteWithoutResults();
        }

        private void AddHas_commentRelation(string postId, string commentId)
        {
            _client.Cypher
                .Match("(p:Post)", "(c:Comment)")
                .Where((Post p) => p.Id == postId)
                .AndWhere((Comment c) => c.Id == commentId)
                .CreateUnique("(p)-[:Has_comment]->(c)")
                .ExecuteWithoutResults();
        }


        internal int LikeComment(string userId, string commentId)
        {
           var data = _client.Cypher
                .Match("(u:User)", "(c:Comment)")
                .Where((User u) => u.Id == userId)
                .AndWhere((Comment c) => c.Id == commentId)
                .Merge("(u)-[:Like]->(c)")
                .OnCreate()
                .Set("c.Likes = c.Likes + 1")
                .Return((c) => c.As<Comment>())
                .Results.FirstOrDefault();

            return data.Likes;
        }

        internal void AddCommentMention(string commentId, ICollection<string> userIds)
        {           
            foreach (var id in userIds)
            {
                _client.Cypher
                    .Match("(c:Comment)", "(u:User)")
                    .Where((Comment c) => c.Id == commentId)
                    .AndWhere((User u) => u.Id == id)
                    .CreateUnique("(c)-[:Mention]->(u)")
                    .ExecuteWithoutResults();
            }
        }

        internal List<Comment> GetComments(string postId)
        {
            var query = _client.Cypher
                .Match("(p:Post)-[:Has_comment]->(c:Comment)")
                .Where((Post p) => p.Id == postId)
                .Return((c) => c.As<Comment>())
                .Results.ToList();

            return query;
        }
        #endregion
    }
}
