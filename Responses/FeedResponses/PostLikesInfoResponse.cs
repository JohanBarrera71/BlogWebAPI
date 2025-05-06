using DemoLinkedIn.Server.Entities;
using DemoLinkedInApi.Entities;

namespace DemoLinkedIn.Server.Responses.FeedResponses;

public class PostLikesInfoResponse
{
    public int PostId { get; set; }

    public List<UserInfoResponse> User { get; set; } = null!;
}