using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationVKTest.Controllers.Requests;
using WebApplicationVKTest.Model.Entities;

namespace WebApplicationVKTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<int>> CreateAsync([FromBody] CreateUserRequest createUserRequest)
        {
            using (ApplicationContext db = new ApplicationContext())
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

                if (group.Count() == 0)
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
        public async Task<ActionResult<UserResponse>> GetByIdAsync(long userId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                /**var response = from user in db.Users.Where(p => p.Id == userId)
                               join gr in db.UserGroups on user.User_group_id equals gr.id
                               join state in db.UserStates on user.User_state_id equals state.id
                               select new
                               {
                                   userId = user.Id,
                                   Login = user.Login,
                                   Password = user.Password,
                                   Created_date = user.Created_date,
                                   GroupId = user.User_group_id,
                                   GroupCode = gr.code,
                                   GroupDescription = gr.description,
                                   StateId = user.User_state_id,
                                   StateCode = state.code,
                                   StateDescription = state.description
                               };*/

                User user = await db.Users.FindAsync(userId);
                if (user != null)
                {
                    return Ok(CreateUserResponse(user));
                }
                return BadRequest(0);
            }
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IList<UserResponse>>> GetAllAsync()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                IList<User> list = await db.Users.ToListAsync();
                if (list != null)
                {
                    IList<UserResponse> response = new List<UserResponse>();
                    foreach (User user in list)
                    {
                        UserResponse el = CreateUserResponse(user);
                        response.Add(el);
                    }
                    return Ok(response);
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

        private UserResponse CreateUserResponse(User user)
        {
            UserResponse response;
            using (ApplicationContext db = new ApplicationContext())
            {
                UserGroup group = db.UserGroups.Find(user.User_group_id);
                UserState state = db.UserStates.Find(user.User_state_id);
                response = new UserResponse()
                {
                    userId = user.Id,
                    Login = user.Login,
                    Password = user.Password,
                    Created_date = user.Created_date,
                    GroupId = user.User_group_id,
                    GroupCode = group.code,
                    GroupDescription = group.description,
                    StateId = user.User_state_id,
                    StateCode = state.code,
                    StateDescription = state.description
                };
            }
            return response;
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