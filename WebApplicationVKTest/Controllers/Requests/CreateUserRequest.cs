using WebApplicationVKTest.Model.Entities;

namespace WebApplicationVKTest.Controllers.Requests
{
    public class CreateUserRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Created_date { get; set; }
        public int User_group_id { get; set; }
    }
}
