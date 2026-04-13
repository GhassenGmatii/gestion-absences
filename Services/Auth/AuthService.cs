using GestionAbsences.DTOs.Auth;
using GestionAbsences.Helpers;
using GestionAbsences.Models;
using GestionAbsences.Models.Enums;
using GestionAbsences.Repositories;

namespace GestionAbsences.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated.");

            var token = JwtHelper.GenerateToken(user, _configuration);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new TokenResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var minutes) ? minutes : 60),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = new Models.User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
                Role = UserRole.Etudiant,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var token = JwtHelper.GenerateToken(user, _configuration);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new TokenResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var minutes) ? minutes : 60),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = JwtHelper.GetPrincipalFromExpiredToken(token, _configuration);

            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token has expired.");

            var newToken = JwtHelper.GenerateToken(user, _configuration);
            var newRefreshToken = JwtHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new TokenResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var minutes) ? minutes : 60),
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
