namespace API.Helpers
{
    public class ConnectionRequestParams : PaginationParams
    {
        public int UserId { get; set; }
        public string predicate { get; set; }
    }
}