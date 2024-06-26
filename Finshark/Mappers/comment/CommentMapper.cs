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
    }
}
