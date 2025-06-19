using api.Data;
using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(int stockId, string userId, Comment comment)
        {
            comment.StockId = stockId;
            comment.AppUserId = userId;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;

        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await GetByIdAsync(id);

            if (commentModel == null) return null;

            _context.Comments.Remove(commentModel);

            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var data = _context.Comments.Include(x => x.AppUser).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                data = data.Where(x => x.Stock.Symbol == query.Symbol);
            }

            if (query.IsDescending)
            {
                data = data.OrderByDescending(x => x.CreatedOn);
            }

            return await data.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> StockExists(int stockId)
        {
            return await _context.Stocks.AnyAsync(x=> x.Id == stockId);
        }

        public async Task<Comment?> UpdateAsync(int id, CommentUpdate comment)
        {
            var commentModel = await GetByIdAsync(id);

            if (commentModel == null) return null;


            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;

            await _context.SaveChangesAsync();

            return commentModel;
        }
    }
}
