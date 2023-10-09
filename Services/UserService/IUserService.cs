
namespace UserApp.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers(string? searchString = null, string? sortOrder = null, int page = 1);
        Task<User> GetUser(int id);
        Task<List<User>> AddUser(User user);
        Task<User>? AddRole(int UserId, int RoleId);
        Task<List<User>?> UpdateUser(int id, User request);
        Task<List<User>?> DeleteUser(int id);
    }
}