using DemoLinkedIn.Server.DTOs.FeedDTOs;
using DemoLinkedIn.Server.Responses.FeedResponses;
using DemoLinkedInApi.Responses;

namespace DemoLinkedIn.Server.Repositories.Contracts
{
    public interface IFeed
    {
        Task<GeneralResponse<object>> CreatePostAsync(PostDTO postDto, string userId);

        Task<GeneralResponse<object>> EditPostAsync(int postId, string userId, PostDTO postDto);

        Task<GeneralResponse<List<PostResponse>>> GetUserPostsAsync(string userId); // For the Activity Feed of user

        Task<GeneralResponse<List<PostResponse>>> GetFeedAllPosts(); // For the Home Feed

        Task<GeneralResponse<PostResponse>> GetPostById(int postId); // When clicking on a post with an image or video

        Task<GeneralResponse<object>> DeletePostAsync(int postId, string userId);

        Task<GeneralResponse<object>> LikePostAsync(int postId, string userId);

        Task<GeneralResponse<PostLikesInfoResponse>> GetLikesByPostId(int postId);

        Task<GeneralResponse<object>> AddCommentAsync(int postId, CommentDto commentDto, string userId);

        Task<GeneralResponse<object>> EditCommentAsync(int commentId, CommentDto commentDto, string userId);
        
        Task<GeneralResponse<object>> DeleteCommentAsync(int commentId, string userId);

    }
}
