using Finshark.Data;
using Finshark.Interfaces;
using Finshark.Models;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllAsync()
        {
           return await _context.Comments.ToListAsync();
        }
    }
}
