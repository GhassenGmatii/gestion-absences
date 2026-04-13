using GestionAbsences.Models;
using GestionAbsences.Models.Enums;

namespace GestionAbsences.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}
