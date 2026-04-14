using GestionAbsences.DTOs.Auth;

namespace GestionAbsences.Services.Auth
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
        Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenResponseDto> RefreshTokenAsync(string token, string refreshToken);
    }
}
