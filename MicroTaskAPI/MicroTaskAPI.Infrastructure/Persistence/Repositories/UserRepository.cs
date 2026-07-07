using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByFirebaseUidAsync(string firebaseUid) =>
            await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

        public async Task<List<User>> GetAllAsync() =>
            await _context.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();

        public async Task<List<User>> GetTopWorkersAsync(int count) =>
            await _context.Users
                .Where(u => u.Role == UserRole.Worker)
                .OrderByDescending(u => u.Coin)
                .Take(count)
                .ToListAsync();

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountByRoleAsync(UserRole role) =>
            await _context.Users.CountAsync(u => u.Role == role);

        public async Task<int> SumAllCoinsAsync() =>
            await _context.Users.SumAsync(u => u.Coin);
    }
}