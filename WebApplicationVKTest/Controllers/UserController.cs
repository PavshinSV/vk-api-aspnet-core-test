using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationVKTest.Controllers.Requests;
using WebApplicationVKTest.Model.Entities;

namespace WebApplicationVKTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private PostgreContext db;
        public UserController()
        {
            db = new PostgreContext("appsettings.json");
        }

        [HttpPost("create")]
        public async Task<int> CreateAsync([FromBody] CreateUserRequest createUserRequest)
        {
            int res=0;
            User newUser = new User()
            {
                Login = createUserRequest.Login,
                Password = createUserRequest.Password,
                Created_date = createUserRequest.Created_date,
                User_group_id = createUserRequest.User_group_id,
                User_state_id = createUserRequest.User_state_id
            };
            long representUserId = GetByLogin(newUser.Login);
            if (representUserId < 0)
            {
                await db.Users.AddAsync(newUser);
                res = await db.SaveChangesAsync();
            }
            Thread.Sleep(5000);
            return res;
        }

        [HttpGet("get-by-id")]
        public async Task<User> GetByIdAsync(long userId) 
         {
            return await db.Users.FindAsync(userId);
         }

        [HttpGet("get-all")]
        public async Task<IList<User>> GetAllAsync()
        {
            return await db.Users.ToListAsync();
        }

        [HttpPut("delete")]
        public async Task<int> DeleteAsync(long userId)
        {
            User user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.User_state_id = 1;
                db.Users.Update(user);
                return await db.SaveChangesAsync();
            }
            return 0;
        }


        private long GetByLogin(string login)
        {
            var users = db.Users.Where(p => p.Login == login);
            if (users.Count() > 0)
            {
                return users.First().Id;
            }
            else
            {
                return -1;
            }
        }

    }
}