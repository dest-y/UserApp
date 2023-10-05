using Microsoft.EntityFrameworkCore;
using UserApp.Data;

namespace UserApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<User>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>?> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            return user;
        }

        public async Task<List<User>?> UpdateUser(int id, User request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            user.Name = request.Name;
            user.Email = request.Email;
            user.Age = request.Age;

            await _context.SaveChangesAsync();

            return await _context.Users.ToListAsync();
        }
    }
}
