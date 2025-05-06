using DemoLinkedIn.Server.DTOs.FeedDTOs;
using DemoLinkedIn.Server.Repositories.Contracts;
using DemoLinkedInApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoLinkedIn.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserPolicy")]
    public class FeedController(IFeed feedService) : ControllerBase
    {
        [HttpGet("/posts/feed")]
        public async Task<IActionResult> GetFeedPostsAsync()
        {
            var result = await feedService.GetFeedAllPosts();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/user")]
        public async Task<IActionResult> GetUserPostsAsync()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.GetUserPostsAsync(userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/{postId:int}")]
        public async Task<IActionResult> GetPostByIdAsync(int postId)
        {
            var result = await feedService.GetPostById(postId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("/posts")]
        public async Task<IActionResult> CreatePostAsync(PostDTO postDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.CreatePostAsync(postDto, userId!);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("/posts/{postId:int}")]
        public async Task<IActionResult> UpdatePostAsync(int postId, PostDTO postDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.EditPostAsync(postId, userId!, postDto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("/posts/{postId:int}")]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.DeletePostAsync(postId, userId!);
            return StatusCode(result.StatusCode, result);
        }

        // Likes

        [HttpPut("/psts/{postId:int}/like")]
        public async Task<IActionResult> LikePostAsync(int postId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.LikePostAsync(postId, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("/posts/{postId:int}/likes")]
        public async Task<IActionResult> GetPostLikesAsync(int postId)
        {
            var result = await feedService.GetLikesByPostId(postId);
            return StatusCode(result.StatusCode, result);
        }

        // Comments
        [HttpPost("/posts/{postId:int}/comments")]
        public async Task<IActionResult> AddComment(int postId, CommentDto commentDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.AddCommentAsync(postId, commentDto, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("/comments/{commentId:int}")]
        public async Task<IActionResult> EditComment(int commentId, CommentDto commentDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.EditCommentAsync(commentId, commentDto, userId!);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("/comments/{commentId:int}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var result = await feedService.DeleteCommentAsync(commentId, userId!);
            return StatusCode(result.StatusCode, result);
        }
    }
}