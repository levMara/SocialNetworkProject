using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication4.Models;
using static WebApplication4.BL.ManagerBase;

namespace WebApplication4.BL
{
    public class PostsManager: ManagerBase
    {

        private static WebAPIAccess socialServiceAccess = new WebAPIAccess("http://localhost:56139/api/");//TODO


        #region Result Models

        public class GetUserFeedResult : ResultBase
        {
            public IEnumerable<PostWithCommentsModel> Feed { get; set; }

            public GetUserFeedResult() { }
            public GetUserFeedResult(bool success, IEnumerable<PostWithCommentsModel> feed, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                Feed = feed;
            }

        }

        public class GetPostCommentsResult : ResultBase
        {
            public IEnumerable<CommentModel> Comments { get; set; }

            public GetPostCommentsResult() { }
            public GetPostCommentsResult(bool success, IEnumerable<CommentModel> comments, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                Comments = comments;
            }

        }

        public class LikePostResult : ResultBase
        {
            public int NewLikesCount { get; set; }
            public LikePostResult() { }


            public LikePostResult(bool success, int newLikesCount, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                this.NewLikesCount = newLikesCount;
            }

        }

        public class LikeCommentResult : LikePostResult
        {
            public LikeCommentResult() { }
            public LikeCommentResult(bool success, int newLikesCount, string userErrorMessage = null) : base(success, newLikesCount, userErrorMessage)
            {
            }
        }

        public class GetPostByIdResult : ResultBase
        {
            public GetPostByIdResult() { }
            public GetPostByIdResult(bool success, PostModel post, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                this.Post = post;
            }
            public PostModel Post { get; set; }
        }

        public class GetPostUploaderIdResult : ResultBase
        {
            public GetPostUploaderIdResult() { }
            public GetPostUploaderIdResult(bool success, string uploaderId, string userErrorMessage = null) : base(success, userErrorMessage)
            {
                this.UploaderId = uploaderId;
            }
            public string UploaderId { get; set; }
        }

        #endregion

        public static async Task<GetPostByIdResult> GetPostById(string userToken, string postId)
        {
            var result = await socialServiceAccess.GetData <PostModel> ($"post/get?token={userToken}&postid={postId}");
            if (result.Item1.IsSuccessStatusCode)
                return new GetPostByIdResult(true, result.Item2);
            else
                return (await (ReturnErrorResult<GetPostByIdResult>(result.Item1, "failed to get post")));
        }

        public static async Task<bool> UploadPost(string userToken, PostModel post)
        {
            var result = await socialServiceAccess.PostData($"post/add?token={userToken}", post);
            return result.IsSuccessStatusCode;

        }

         public static async Task<GetPostUploaderIdResult> GetPostUploaderId(string userToken, string postId)
        {
            var result = await socialServiceAccess.GetData<string>($"post/GetUploaderId?token={userToken}&postid={postId}");
            if (result.Item1.IsSuccessStatusCode)
                return new GetPostUploaderIdResult(true, result.Item2);
            else
                return (await (ReturnErrorResult<GetPostUploaderIdResult>(result.Item1, "failed to get post uploader id")));

        }
        
        public static async Task<LikePostResult> LikePost(string userToken, string postId)
        {
            var result = await socialServiceAccess.GetData<int>($"post/LikePost?token={userToken}&postid={postId}");
            if (result.Item1.IsSuccessStatusCode)
            {
                var uploaderIdResult = await GetPostUploaderId(userToken, postId);
                if (uploaderIdResult.Success)
                {
                    await NotificationManager.PostLiked(userToken, uploaderIdResult.UploaderId);
                }
                return new LikePostResult(true, result.Item2);
            }
            else
                return (await (ReturnErrorResult<LikePostResult>(result.Item1, "failed to add like to post")));
        }

        public static async Task<LikeCommentResult> LikeComment(string userToken, string commentId)
        {
            var result = await socialServiceAccess.GetData<int>($"comment/LikeComment?token={userToken}&commentId={commentId}");
            if (result.Item1.IsSuccessStatusCode)
                return new LikeCommentResult(true, result.Item2);
            else
                return (await (ReturnErrorResult<LikeCommentResult>(result.Item1, "failed to add like to comment")));
        }

        public static async Task<bool> UploadComment(string userToken, CommentModel comment,string postId)
        {
            var result = await socialServiceAccess.PostData($"Comment/Comment?token={userToken}&postid={postId}", comment);
            return result.IsSuccessStatusCode;

        }

        private static async Task<GetPostCommentsResult> GetPostComments(string userToken,string postId)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<CommentModel>>($"comment/GetComments?token={userToken}&postid={postId}");
            if (result.Item1.IsSuccessStatusCode)
                return new GetPostCommentsResult(true, result.Item2);
            else
                return (await (ReturnErrorResult<GetPostCommentsResult>(result.Item1, "failed to get post comments")));
        }

        internal static async Task FillPostsWithComments(string userToken, IEnumerable<PostWithCommentsModel> posts)
        {
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    var commentsResult = await GetPostComments(userToken, post.Id);
                    if (commentsResult.Success)
                        post.Comments = commentsResult.Comments;
                }
            }
        }

        public static async Task<GetUserFeedResult> GetUserFeed(string userToken)
        {
            var result = await socialServiceAccess.GetData<IEnumerable<PostWithCommentsModel>>($"feed/getfeed?token={userToken}");
            if (result.Item1.IsSuccessStatusCode)
            {
                var posts = result.Item2;
                await FillPostsWithComments(userToken, posts);
                return new GetUserFeedResult(true, posts);
            }
            else
                return (await (ReturnErrorResult<GetUserFeedResult>(result.Item1, "failed to get feed")));
        }

    }
}