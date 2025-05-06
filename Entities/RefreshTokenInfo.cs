namespace DemoLinkedIn.Server.Entities
{
    public class RefreshTokenInfo
    {
        public int Id { get; set; }
        public string?  RefreshToken { get; set; }
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }
        public bool Revoked { get; set; } = false;
    }
}
