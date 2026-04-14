using GestionAbsences.DTOs.User;
using GestionAbsences.Helpers;
using GestionAbsences.Models.Enums;
using GestionAbsences.Repositories;

namespace GestionAbsences.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new ArgumentException($"Invalid role: {dto.Role}");

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = new Models.User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.Email))
            {
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Id != id)
                    throw new InvalidOperationException("A user with this email already exists.");

                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Role))
            {
                if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                    throw new ArgumentException($"Invalid role: {dto.Role}");

                user.Role = role;
            }

            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }

        public async Task<UserResponseDto?> ChangeRoleAsync(int id, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                throw new ArgumentException($"Invalid role: {role}");

            user.Role = parsedRole;
            await _userRepository.UpdateAsync(user);
            return MapToDto(user);
        }

        private static UserResponseDto MapToDto(Models.User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
