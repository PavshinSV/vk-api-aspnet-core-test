namespace WebApplicationVKTest.Model.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Created_date { get; set; }
        public int User_group_id { get; set; }
        public int User_state_id { get; set; }
    }
}
