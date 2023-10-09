using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
            return await _context.Users.Include(c => c.Roles).ToListAsync();
        }

        public async Task<User>? AddRole(int UserId, int RoleId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
                return null;

            var role = await _context.Roles.FindAsync(RoleId);
            if (role == null)
                return null;

            user.Roles.Add(role);
            await _context.SaveChangesAsync();

            return _context.Users.Include(c => c.Roles).FirstOrDefault(c => c.Id == UserId);
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

        public async Task<List<User>> GetAllUsers(string? searchString = null, string? sortOrder = null, int page = 1)
        {
            IQueryable<User> result = _context.Users
                .Include(t => t.Roles);

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result
                   .Where(u => u.Name.Contains(searchString) ||
                       u.Email.Contains(searchString) ||
                       u.Roles.Any(c => c.Name.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    result = result.OrderByDescending(s => s.Name);
                    break;
                case "age":
                    result = result.OrderBy(s => s.Age);
                    break;
                case "email_desc":
                    result = result.OrderByDescending(s => s.Email); ;
                    break;
                default:
                    result = result.OrderBy(s => s.Name).Include(c => c.Roles.OrderBy(f => f.Name));
                    break;
            }

            var pageResults = 5f;
            var pageCount = Math.Ceiling(result.Count() / pageResults);

            result = result.Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults);

            return await result.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(c => c.Roles).FirstOrDefaultAsync(c => c.Id == id);
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

        public class UniqEmailAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value,
                ValidationContext validationContext)
            {
                var context = validationContext.GetService(typeof(DataContext)) as DataContext;
                if (!context.Users.Any(a => a.Email == value.ToString()))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"This Email already exists");
            }
        }
    }
}
