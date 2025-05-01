using System.Text;
using APP.Auth.Services;
using APP.Users.Context;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace APP.Auth.Features.Login
{
    public class UserLoginRequest : Request, IRequest<CommandResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginHandler : Handler, IRequestHandler<UserLoginRequest, CommandResponse>
    {
        private readonly UsersDb _db;
        private readonly AuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLoginHandler(UsersDb db, AuthService authService, IHttpContextAccessor httpContextAccessor)
            : base(System.Globalization.CultureInfo.CurrentCulture)
        {
            _db = db;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CommandResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var hashedPassword = HashPassword(request.Password);

            var user = await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == hashedPassword);

            if (user == null)
                return Error("Invalid username or password.");

            var token = _authService.GenerateJwtToken(user);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Success(token, user.Id);

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
