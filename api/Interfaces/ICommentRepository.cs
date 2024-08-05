using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        public Task<Comment> AddAsync(int stockId, string userId, Comment comment);
        public Task<Comment?> UpdateAsync(int id, CommentUpdate comment);
        public Task<Comment?> DeleteAsync(int id);
        public Task<Comment?> GetByIdAsync(int id);
        public Task<List<Comment>> GetAllAsync();
        public Task<Boolean> StockExists(int stockId);
    }
}
