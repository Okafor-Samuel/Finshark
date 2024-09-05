using Finshark.Data;
using Finshark.Dtos.comment;
using Finshark.Dtos.stock;
using Finshark.Extension;
using Finshark.Interfaces;
using Finshark.Mappers.comment;
using Finshark.Mappers.stock;
using Finshark.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());

            return Ok(commentDto);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null) 
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _stockRepo.stockExists(stockId)) 
            {
                return BadRequest("Stock does not exist");
            }
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            commentModel.AppUserId = appUser.Id;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto updateDto) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
            if (comment == null) 
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment does not exist");
            }
            return Ok(commentModel.ToCommentDto());
        }
    }
}
    