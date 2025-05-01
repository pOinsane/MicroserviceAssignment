using APP.Users.Context;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace APP.Auth.Features.Auth
{
    public class UserRegisterRequest : Request, IRequest<CommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class UserRegisterHandler : Handler, IRequestHandler<UserRegisterRequest, CommandResponse>
    {
        private readonly UsersDb _db;

        public UserRegisterHandler(UsersDb db) : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
        }

        public async Task<CommandResponse> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (existingUser != null)
                return Error("Username already taken.");

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
            if (role == null)
                return Error("Default role 'User' does not exist. Please create it first.");

            var hashedPassword = HashPassword(request.Password);

            var user = new User
            {
                UserName = request.UserName,
                Password = hashedPassword,
                Name = request.Name,
                Surname = request.Surname,
                RoleId = role.Id,
                IsActive = true,
                RegistrationDate = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Success("User registered successfully", user.Id);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
