using Microsoft.EntityFrameworkCore;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Infrastructure.Persistence.Context;

namespace MicroTaskAPI.Infrastructure.Persistence.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _context;

        public SubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Submission?> GetByIdAsync(int id) =>
            await _context.Submissions
                .Include(s => s.Task)
                .Include(s => s.Worker)
                .Include(s => s.Buyer)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<List<Submission>> GetByWorkerEmailAsync(string workerEmail) =>
            await _context.Submissions
                .Include(s => s.Worker)
                .Include(s => s.Buyer)
                .Where(s => s.Worker.Email == workerEmail)
                .OrderByDescending(s => s.CurrentDate)
                .ToListAsync();

        public async Task<(List<Submission> Items, int TotalCount)> GetByWorkerEmailPagedAsync(string workerEmail, int page, int size)
        {
            var query = _context.Submissions
                .Include(s => s.Worker)
                .Include(s => s.Buyer)
                .Where(s => s.Worker.Email == workerEmail)
                .OrderByDescending(s => s.CurrentDate);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<Submission>> GetPendingByBuyerEmailAsync(string buyerEmail) =>
            await _context.Submissions
                .Include(s => s.Worker)
                .Include(s => s.Buyer)
                .Where(s => s.Buyer.Email == buyerEmail && s.Status == SubmissionStatus.Pending)
                .OrderByDescending(s => s.CurrentDate)
                .ToListAsync();

        public async Task<List<Submission>> GetApprovedByWorkerEmailAsync(string workerEmail) =>
            await _context.Submissions
                .Include(s => s.Worker)
                .Include(s => s.Buyer)
                .Where(s => s.Worker.Email == workerEmail && s.Status == SubmissionStatus.Approved)
                .OrderByDescending(s => s.CurrentDate)
                .ToListAsync();

        public async Task<Submission> AddAsync(Submission submission)
        {
            await _context.Submissions.AddAsync(submission);
            await _context.SaveChangesAsync();
            return submission;
        }

        public async Task UpdateAsync(Submission submission)
        {
            _context.Submissions.Update(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountByWorkerAsync(int workerId) =>
            await _context.Submissions.CountAsync(s => s.WorkerId == workerId);

        public async Task<int> CountByWorkerAndStatusAsync(int workerId, SubmissionStatus status) =>
            await _context.Submissions.CountAsync(s => s.WorkerId == workerId && s.Status == status);

        public async Task<int> SumEarningByWorkerAsync(int workerId) =>
            await _context.Submissions
                .Where(s => s.WorkerId == workerId && s.Status == SubmissionStatus.Approved)
                .SumAsync(s => s.PayableAmount);
    }
}