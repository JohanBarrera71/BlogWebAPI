using DemoLinkedIn.Server.DTOs.FeedDTOs;
using DemoLinkedIn.Server.Repositories.Contracts;
using DemoLinkedIn.Server.Responses.FeedResponses;
using DemoLinkedInApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoLinkedIn.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserPolicy")]
    public class FeedController(IFeed feedService) : ControllerBase
    {
        [HttpGet("/posts/feed")]
        [SwaggerOperation(
            Summary = "Get all posts",
            Description = "Retrieves all posts from the feed.",
            OperationId = "GetAllPosts",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Posts retrieved successfully.",
            typeof(GeneralResponse<List<PostResponse>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No posts found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> GetFeedPostsAsync()
        {
            var result = await feedService.GetFeedAllPosts();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/user")]
        [SwaggerOperation(
            Summary = "Get all user's posts",
            Description = "Retrieves all posts from the user's feed.",
            OperationId = "GetUserPosts",
            Tags = new[] { "User Account" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Posts retrieved successfully.",
            typeof(GeneralResponse<List<PostResponse>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User or posts not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> GetUserPostsAsync()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.GetUserPostsAsync(userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/{postId:int}")]
        [SwaggerOperation(
            Summary = "Get post by ID",
            Description = "Retrieves a post by its ID.",
            OperationId = "GetPostById",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Post retrieved successfully.",
            typeof(GeneralResponse<List<PostResponse>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Post not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> GetPostByIdAsync(int postId)
        {
            var result = await feedService.GetPostById(postId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("/posts")]
        [SwaggerOperation(
            Summary = "Create a new post",
            Description = "Creates a new post.",
            OperationId = "CreatePost",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Post created successfully.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid post data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found. The user may not exist or the token is invalid.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> CreatePostAsync(PostDTO postDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.CreatePostAsync(postDto, userId!);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("/posts/{postId:int}")]
        [SwaggerOperation(
            Summary = "Update a post",
            Description = "Updates an existing post while the user is the owner.",
            OperationId = "UpdatePost",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Post updated successfully.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid post data. The user token may be invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User or post not found.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized access to edit the post. The user is not the owner.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> UpdatePostAsync(int postId, PostDTO postDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.EditPostAsync(postId, userId!, postDto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("/posts/{postId:int}")]
        [SwaggerOperation(
            Summary = "Delete a post",
            Description = "Deletes a post while the user is the owner.",
            OperationId = "DeletePost",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Post deleted successfully.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid post data. The user token may be invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User or post not found.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized access to delete the post. The user is not the owner.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.DeletePostAsync(postId, userId!);
            return StatusCode(result.StatusCode, result);
        }

        // Likes

        [HttpPut("/psts/{postId:int}/like")]
        [SwaggerOperation(
            Summary = "Like a post",
            Description = "Likes any post of any user.",
            OperationId = "LikePost",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Post liked successfully.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User or post not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> LikePostAsync(int postId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.LikePostAsync(postId, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/{postId:int}/likes")]
        [SwaggerOperation(
            Summary = "Get post likes",
            Description = "Retrives all likes of a post by its ID.",
            OperationId = "GetPostLikes",
            Tags = new[] { "Feed" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Liked info post.",
            typeof(GeneralResponse<PostLikesInfoResponse>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Post not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> GetPostLikesAsync(int postId)
        {
            var result = await feedService.GetLikesByPostId(postId);
            return StatusCode(result.StatusCode, result);
        }

        // Comments
        [HttpPost("/posts/{postId:int}/comments")]
        [SwaggerOperation(
            Summary = "Add a comment",
            Description = "Adds a comment to a post.",
            OperationId = "AddComment",
            Tags = new[] { "Comments" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Comment created.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Post or user not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> AddComment(int postId, CommentDto commentDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.AddCommentAsync(postId, commentDto, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("/comments/{commentId:int}")]
        [SwaggerOperation(
            Summary = "Edit a comment",
            Description = "Edits a comment on a post while the user is the owner.",
            OperationId = "EditComment",
            Tags = new[] { "Comments" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Comment edited.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Comment or user not found.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized access to edit the comment. The user is not the owner.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> EditComment(int commentId, CommentDto commentDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.EditCommentAsync(commentId, commentDto, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("/comments/{commentId:int}")]
        [SwaggerOperation(
            Summary = "Delete a comment",
            Description = "Deletes a comment on a post while the user is the owner.",
            OperationId = "DeleteComment",
            Tags = new[] { "Comments" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Comment deleted.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Comment or user not found.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Unauthorized access to delete the comment. The user is not the owner.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.DeleteCommentAsync(commentId, userId!);
            return StatusCode(result.StatusCode, result);
        }
    }
}