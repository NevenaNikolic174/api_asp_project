using Microsoft.EntityFrameworkCore;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Core.FakeActor
{
    public class FakeActor : IApplicationActor
    {
        public int Id => 1;
        public int RoleId => 1;
        public string Username => "mika123";
        public string Email => "mika123@gmail.com";
        public string FirstName => "Mika";
        public string LastName => "Mikic";
        public IEnumerable<int> AllowedUseCases => new List<int> { 500 };
    }

    public class AdminFakeActor : IApplicationActor
    {
        public int Id => 2;
        public int RoleId => 2;
        public string Username => "admin123";
        public string Email => "admin123@gmail.com";
        public string FirstName => "Nevena";
        public string LastName => "Nikolic";
        public IEnumerable<int> AllowedUseCases => Enumerable.Range(1, 1000);
    }

    public class UserService
    {
        private readonly MyDBContext _context;

        public UserService(MyDBContext context)
        {
            _context = context;
        }

        public bool IsUserAdmin(int userId)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
            return user?.Role?.Name == "Admin";
        }
    }
}
