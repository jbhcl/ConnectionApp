namespace API.Entities
{
    public class UserConnectionRequest
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser RequestedUser { get; set; }
        public int RequestedUserId { get; set; }       
    }
}