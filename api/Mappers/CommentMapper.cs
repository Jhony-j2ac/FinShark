using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static Comment FromCreateDtoToComment(this CreateComment commentDto, int stockId)
        {
            return new Comment()
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId

            };
        }

        public static CommentDto FromCommentToDto(this Comment comment)
        {
            return new CommentDto()
            {
                Title = comment.Title,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId,
                AppUser = comment.AppUser,
                Content = comment.Content,

            };
        }


    }
}
