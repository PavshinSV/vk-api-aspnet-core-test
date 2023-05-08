namespace WebApplicationVKTest.Model.Entities
{
    public class UserResponse
    {
        public long userId { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Created_date { get; set; }
        public int GroupId { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupDescription { get; set; }
        public int StateId { get; set; }
        public string? StateCode { get; set; }
        public string? StateDescription { get; set; }
    }
}
