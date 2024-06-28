using Finshark.Data;
using Finshark.Dtos.comment;
using Finshark.Dtos.stock;
using Finshark.Interfaces;
using Finshark.Mappers.comment;
using Finshark.Mappers.stock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        { 
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null) 
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto) 
        {
            if (!await _stockRepo.stockExists(stockId)) 
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel}, commentModel.ToCommentDto());
        }

    }
}
