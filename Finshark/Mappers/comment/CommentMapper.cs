using Finshark.Dtos.comment;
using Finshark.Models;
using System.Runtime.CompilerServices;

namespace Finshark.Mappers.comment
{
    public  static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel) 
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedAt = commentModel.CreatedAt,
                StockId = commentModel.StockId
            };
        }

        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
        {
            return new Comment
            {
                
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
    }
}
