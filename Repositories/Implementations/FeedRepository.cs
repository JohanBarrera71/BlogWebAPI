using DemoLinkedIn.Server.DTOs.FeedDTOs;
using DemoLinkedIn.Server.Entities;
using DemoLinkedIn.Server.Repositories.Contracts;
using DemoLinkedIn.Server.Responses;
using DemoLinkedIn.Server.Responses.FeedResponses;
using DemoLinkedInApi.Data;
using DemoLinkedInApi.Entities;
using DemoLinkedInApi.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DemoLinkedIn.Server.Repositories.Implementations
{
    public class FeedRepository : IFeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;

        public FeedRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task<GeneralResponse<object>> CreatePostAsync(PostDTO postDto, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new GeneralResponse<object>(false, "User id is required.", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User does not exist.", StatusCodes.Status404NotFound);

            var post = new Post
            {
                Content = postDto.Content,
                Picture = postDto.Picture,
                CreationDate = DateTime.UtcNow,
                UserId = userId
            };

            await _appDbContext.Posts.AddAsync(post);
            await _appDbContext.SaveChangesAsync();

            return new GeneralResponse<object>(true, "Post created successfully.", StatusCodes.Status201Created);
        }

        public async Task<GeneralResponse<object>> EditPostAsync(int postId, string userId, PostDTO postDto)
        {
            if (string.IsNullOrEmpty(userId))
                return new GeneralResponse<object>(false, "Invalid user ID.", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            var post = await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
                return new GeneralResponse<object>(false, "Post not found.", StatusCodes.Status404NotFound);

            if (post.UserId != userId)
                return new GeneralResponse<object>(false, "Unauthorized to edit this post.",
                    StatusCodes.Status403Forbidden);

            post.Content = postDto.Content;
            post.Picture = postDto.Picture;
            post.UpdatedDate = DateTime.UtcNow;

            _appDbContext.Posts.Update(post);
            await _appDbContext.SaveChangesAsync();

            return new GeneralResponse<object>(true, "Post updated successfully.");
        }

        public async Task<GeneralResponse<List<PostResponse>>> GetUserPostsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<List<PostResponse>>(false, "User not found.", StatusCodes.Status404NotFound);

            var posts = await _appDbContext.Posts
                .Include(post => post.User)
                .Include(post => post.Comments)
                .Include(post => post.PostLikes)
                .Where(post => post.UserId == user.Id)
                .ToListAsync();

            if (posts.Count == 0)
                return new GeneralResponse<List<PostResponse>>(false, "No posts found.", StatusCodes.Status404NotFound);

            var postResponses = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                Content = p.Content,
                Picture = p.Picture,
                CreationDate = p.CreationDate,
                UpdatedDate = p.UpdatedDate,
                User = new UserInfoResponse
                {
                    Id = p.User.Id,
                    UserName = p.User.UserName,
                    Email = p.User.Email,
                    EmailConfirmed = p.User.EmailConfirmed,
                    Headline = p.User.Headline,
                    ProfilePicture = p.User.ProfilePicture,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    Company = p.User.Company,
                    Position = p.User.Position,
                    Country = p.User.Country,
                    City = p.User.City,
                    Website = p.User.Website,
                    ProfileComplete = p.User.ProfileComplete
                },
                CommentsCount = p.Comments.Count,
                LikesCount = p.PostLikes.Count
            }).ToList();

            return new GeneralResponse<List<PostResponse>>(true, "Posts retrieved successfully.", Data: postResponses);
        }

        public async Task<GeneralResponse<List<PostResponse>>> GetFeedAllPosts()
        {
            var posts = await _appDbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .ToListAsync();

            if (posts.Count == 0)
                return new GeneralResponse<List<PostResponse>>(false, "No posts found.", StatusCodes.Status404NotFound);

            var postResponses = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                Content = p.Content,
                Picture = p.Picture,
                CreationDate = p.CreationDate,
                UpdatedDate = p.UpdatedDate,
                User = new UserInfoResponse
                {
                    Id = p.User.Id,
                    UserName = p.User.UserName,
                    Email = p.User.Email,
                    Headline = p.User.Headline,
                    EmailConfirmed = p.User.EmailConfirmed,
                    ProfilePicture = p.User.ProfilePicture,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    Company = p.User.Company,
                    Position = p.User.Position,
                    Country = p.User.Country,
                    City = p.User.City,
                    Website = p.User.Website,
                    ProfileComplete = p.User.ProfileComplete
                },
                CommentsCount = p.Comments.Count,
                LikesCount = p.PostLikes.Count
            }).ToList();

            return new GeneralResponse<List<PostResponse>>(true, "Posts retrieved successfully.", Data: postResponses);
        }

        public async Task<GeneralResponse<PostResponse>> GetPostById(int postId)
        {
            try
            {
                var post = await _appDbContext.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .Include(p => p.PostLikes)
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post is null)
                    return new GeneralResponse<PostResponse>(false, "Post not found.", StatusCodes.Status404NotFound);

                var postResponse = new PostResponse
                {
                    Id = post.Id,
                    Content = post.Content,
                    Picture = post.Picture,
                    CreationDate = post.CreationDate,
                    UpdatedDate = post.UpdatedDate,
                    User = new UserInfoResponse
                    {
                        Id = post.User.Id,
                        UserName = post.User.UserName,
                        Email = post.User.Email,
                        EmailConfirmed = post.User.EmailConfirmed,
                        Headline = post.User.Headline,
                        ProfilePicture = post.User.ProfilePicture,
                        FirstName = post.User.FirstName,
                        LastName = post.User.LastName,
                        Company = post.User.Company,
                        Position = post.User.Position,
                        Country = post.User.Country,
                        City = post.User.City,
                        Website = post.User.Website,
                        ProfileComplete = post.User.ProfileComplete
                    },
                    Comments = post.Comments.Select(c => new CommentInfoResponse
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreationDate = c.CreationDate,
                        UpdatedDate = c.UpdateDate,
                        UserId = c.UserId,
                        PostId = c.PostId ?? 0,
                        User = new UserInfoResponse
                        {
                            Id = c.User.Id,
                            UserName = c.User.UserName,
                            Email = c.User.Email,
                            EmailConfirmed = c.User.EmailConfirmed,
                            Headline = c.User.Headline,
                            ProfilePicture = c.User.ProfilePicture,
                            FirstName = c.User.FirstName,
                            LastName = c.User.LastName,
                            Company = c.User.Company,
                            Position = c.User.Position,
                            Country = post.User.Country,
                            City = post.User.City,
                            Website = post.User.Website,
                            ProfileComplete = c.User.ProfileComplete
                        }
                    }).ToList(),
                    CommentsCount = post.Comments.Count,
                    LikesCount = post.PostLikes.Count
                };

                return new GeneralResponse<PostResponse>(true, "Post retrieved.", Data: postResponse);
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PostResponse>(false, $"An error occurred: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<GeneralResponse<object>> DeletePostAsync(int postId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new GeneralResponse<object>(false, "Invalid user ID.", StatusCodes.Status400BadRequest);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            var post = await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return new GeneralResponse<object>(false, "Post not found.", StatusCodes.Status404NotFound);
            if (post.UserId != userId)
                return new GeneralResponse<object>(false, "Unauthorized to delete this post.",
                    StatusCodes.Status403Forbidden);

            _appDbContext.Posts.Remove(post);
            await _appDbContext.SaveChangesAsync();
            return new GeneralResponse<object>(true, "Post deleted successfully.");
        }

        public async Task<GeneralResponse<object>> LikePostAsync(int postId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            var post = await _appDbContext.Posts.Include(post => post.PostLikes)
                .FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return new GeneralResponse<object>(false, "Post not found.", StatusCodes.Status404NotFound);

            if (post.PostLikes.Any(p => p.UserId == userId))
            {
                await _appDbContext.PostLikes.Where(p => p.PostId == post.Id && p.UserId == user.Id)
                    .ExecuteDeleteAsync();
            }
            else
            {
                _appDbContext.PostLikes.Add(new PostLike { PostId = post.Id, UserId = user.Id });
            }

            await _appDbContext.SaveChangesAsync();

            return new GeneralResponse<object>(true, "Post liked successfully.");
        }

        public async Task<GeneralResponse<PostLikesInfoResponse>> GetLikesByPostId(int postId)
        {
            var post = await _appDbContext.Posts
                .Include(p => p.PostLikes)
                .ThenInclude(pl => pl.User)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post is null)
                return new GeneralResponse<PostLikesInfoResponse>(false, "Post not found.",
                    StatusCodes.Status404NotFound);

            var result = new PostLikesInfoResponse
            {
                PostId = post.Id,
                User = post.PostLikes.Select(pl => new UserInfoResponse
                {
                    Id = pl.User.Id,
                    UserName = pl.User.UserName,
                    Headline = pl.User.Headline
                }).ToList()
            };

            return new GeneralResponse<PostLikesInfoResponse>(true, "Liked info post.", Data: result);
        }

        public async Task<GeneralResponse<object>> AddCommentAsync(int postId, CommentDto commentDto, string userId)
        {
            var post = await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
                return new GeneralResponse<object>(false, "Post not found.", StatusCodes.Status404NotFound);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            var addComment = new Comment
            {
                Content = commentDto.Content,
                PostId = post.Id,
                UserId = user.Id
            };

            _appDbContext.Comments.Add(addComment);
            await _appDbContext.SaveChangesAsync();

            return new GeneralResponse<object>(true, "Comment created.", StatusCode: StatusCodes.Status201Created);
        }

        public async Task<GeneralResponse<object>> EditCommentAsync(int commentId, CommentDto commentDto, string userId)
        {
            var comment = await _appDbContext.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
                return new GeneralResponse<object>(false, "Comment not found.", StatusCodes.Status404NotFound);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            if (comment.UserId != user.Id)
                return new GeneralResponse<object>(false, "Unauthorized.", StatusCodes.Status403Forbidden);

            comment.Content = commentDto.Content;
            comment.UpdateDate = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            return new GeneralResponse<object>(true, "Comment edited.");
        }

        public async Task<GeneralResponse<object>> DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await _appDbContext.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment is null)
                return new GeneralResponse<object>(false, "Comment not found.", StatusCodes.Status404NotFound);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            if (comment.UserId != user.Id)
                return new GeneralResponse<object>(false, "Unauthorized.", StatusCodes.Status403Forbidden);

            _appDbContext.Comments.Remove(comment);
            await _appDbContext.SaveChangesAsync();
            return new GeneralResponse<object>(true, "Comment deleted.");
        }
    }
}