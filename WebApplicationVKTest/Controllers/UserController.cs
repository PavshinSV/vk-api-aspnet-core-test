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
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateAsync([FromBody] CreateUserRequest createUserRequest)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                initDb();
                int res = 0;
                User newUser = new User()
                {
                    Login = createUserRequest.Login,
                    Password = createUserRequest.Password,
                    Created_date = createUserRequest.Created_date,
                    User_group_id = createUserRequest.User_group_id,
                    User_state_id = 1
                };

                var group = db.UserGroups.Where(p => p.id == newUser.User_group_id);

                if (group.Count()==0)
                {
                    return BadRequest(0);
                }

                long representUserId = GetByLogin(newUser.Login);
                if (representUserId < 0)
                {
                    var adminUser = db.Users.Where(p => p.User_group_id == 1);
                    if (newUser.User_group_id != 1 || adminUser.Count() == 0)
                    {
                        await db.Users.AddAsync(newUser);
                        res = await db.SaveChangesAsync();
                        Thread.Sleep(5000);
                        return Ok(res);
                    }
                }
                Thread.Sleep(5000);
                return BadRequest(res);
            }
            
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<User>> GetByIdAsync(long userId) 
         {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = await db.Users.FindAsync(userId);
                if (user != null)
                {
                    return Ok(user);
                }
                return BadRequest(0);
            }
         }

        [HttpGet("get-all")]
        public async Task<ActionResult<IList<User>>> GetAllAsync()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                IList<User> list = await db.Users.ToListAsync();
                if (list != null)
                {
                    return Ok(list);
                }
                return BadRequest(0);
            }
        }

        [HttpPut("delete")]
        public async Task<ActionResult<int>> DeleteAsync(long userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = await db.Users.FindAsync(userId);
                if (user != null)
                {
                    user.User_state_id = 2;
                    db.Users.Update(user);
                    return Ok(await db.SaveChangesAsync());
                }
                return BadRequest(0);
            }
        }


        private long GetByLogin(string login)
        {
            using (ApplicationContext db = new ApplicationContext())
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

        private void initDb()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                IList<UserGroup> Groups = db.UserGroups.ToList();
                if (Groups.Count() == 0)
                {
                    UserGroup admins = new UserGroup()
                    {
                        id = 1,
                        code = "Admin",
                        description = "Group of Admins"
                    };

                    UserGroup users = new UserGroup()
                    {
                        id = 2,
                        code = "User",
                        description = "Group of Users"
                    };

                    db.UserGroups.AddRange(admins, users);
                    db.SaveChanges();
                }


                IList<UserState> States = db.UserStates.ToList();
                if (States.Count() == 0)
                {
                    UserState active = new UserState()
                    {
                        id = 1,
                        code = "Active",
                        description = "Active user"
                    };

                    UserState blocked = new UserState()
                    {
                        id = 2,
                         code = "Blocked",
                         description = "Blocked user. Contact with admin"
                    };
                    db.AddRange(active, blocked);
                    db.SaveChanges();
                }
            }
        }

    }
}