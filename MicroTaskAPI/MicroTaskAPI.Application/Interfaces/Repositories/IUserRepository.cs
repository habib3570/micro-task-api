using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByFirebaseUidAsync(string firebaseUid);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetTopWorkersAsync(int count);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<int> CountByRoleAsync(Domain.Enums.UserRole role);
        Task<int> SumAllCoinsAsync();
    }
}