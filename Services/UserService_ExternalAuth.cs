namespace WebApiCore.Services
{
    public interface IUserService_ExternalAuth
    {
        Task<ExternalAuthUser> Authenticate(string username, string password);
        Task<IEnumerable<ExternalAuthUser>> GetAll();
    }

    public class UserService_ExternalAuth : IUserService_ExternalAuth
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<ExternalAuthUser> _users = new List<ExternalAuthUser>
        {
        new ExternalAuthUser { idCompañia = 223, Username = "Ikos", Password = "SaniiKos1757." }
        };

        public async Task<ExternalAuthUser> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" to mimic fetching user from a db
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return user;
        }

        public async Task<IEnumerable<ExternalAuthUser>> GetAll()
        {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            return await Task.Run(() => _users);
        }
    }
}

public class ExternalAuthUser
{
    public int idCompañia { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
