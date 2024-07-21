using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    public class CommentController: ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var comments = await _commentRepository.GetAllAsync();

            var commentList = comments.Select(x => x.FromCommentToDto());
            return Ok(commentList);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var model = await _commentRepository.GetByIdAsync(id);
            if(model == null) return NotFound();

            return Ok(model.FromCommentToDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute]int stockId, [FromBody] CreateComment commentDto)
        {
            if (!await _commentRepository.StockExists(stockId))
                return BadRequest("Stock not exists"); 

            var model = await _commentRepository.AddAsync(stockId, commentDto.FromCreateDtoToComment(stockId));

            return CreatedAtAction( nameof(GetById), new { id = model.Id }, model );
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CommentUpdate commentDto) {
            

            var commentModel = await _commentRepository.UpdateAsync(id, commentDto);
            if (commentModel == null) return BadRequest();

            return Ok(commentModel);

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedModel = await _commentRepository.DeleteAsync(id);
            if (deletedModel == null) return BadRequest();
            return Ok(deletedModel);
        }
    }
}
