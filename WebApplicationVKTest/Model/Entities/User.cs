namespace WebApplicationVKTest.Model.Entities
{
    public class User
    {
        public long id { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        public DateTime created_date { get; set; }
        public long user_group_id { get; set; }
        public long user_state_id { get; set; }

    }
}
