namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = -1;
        public int MaxAge { get; set; } = 999;
        public string Industry { get; set; }
        public string OrderBy { get; set; } = "lastActive";
    }
}