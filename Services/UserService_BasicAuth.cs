namespace WebApiCore.Services
{
    public interface IUserService_BasicAuth
    {
        Task<BasicAuthUser> Authenticate(string username, string password);
        Task<IEnumerable<BasicAuthUser>> GetAll();
    }

    public class UserService_BasicAuth : IUserService_BasicAuth
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<BasicAuthUser> _users = new List<BasicAuthUser>
        {
        new BasicAuthUser { Id = 1, Username = "test", Password = "test" },
        new BasicAuthUser { Id = 2, Username = "SmartClock", Password = "5f7d0a8b7a77e02452939046274cdf4e" },
        new BasicAuthUser { Id = 3, Username = "SmartArea", Password = "632206e5ffe862a3e5ad9f5b46a214d3" },
        new BasicAuthUser { Id = 4, Username = "SmartLock", Password = "e2108f6b2fcc80958f3f09167aa36c8b" },
        new BasicAuthUser { Id = 5, Username = "SmartHub", Password = "3dda71f08a66b927d8067ef757b9a42d" }
        };

        public async Task<BasicAuthUser> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" to mimic fetching user from a db
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return user;
        }

        public async Task<IEnumerable<BasicAuthUser>> GetAll()
        {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            return await Task.Run(() => _users);
        }
    }
}

public class BasicAuthUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
