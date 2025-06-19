using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    public class CommentController: ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IFPMService _fpmService;

        public CommentController(ICommentRepository commentRepository, UserManager<AppUser> userManager, IStockRepository stockRepo, IFPMService fpmService)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
            _stockRepo = stockRepo;
            _fpmService = fpmService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query) {
            var comments = await _commentRepository.GetAllAsync(query);

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

        [HttpPost]
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute]string symbol, [FromBody] CreateComment commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock is null)
            {
                stock = await this._fpmService.FindStockBySymbolAsync(symbol);
                if(stock == null)
                {
                    return BadRequest("Stock not found on fpm");
                }
                await this._stockRepo.AddAsync(stock);

            }

            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);

            var model = await _commentRepository.AddAsync(stock.Id, appUser.Id, commentDto.FromCreateDtoToComment(stock.Id));

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
