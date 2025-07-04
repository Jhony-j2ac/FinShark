﻿using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
            var stocks = await _stockRepository.GetAllAsync(query);           
            return Ok(stocks.Select(x => x.ToStockDto()));
        }


        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var stocks = await _stockRepository.GetByIdAsync(id);
                

            if (stocks == null) return NotFound();
            
            var stockDto = stocks.ToStockDto();

            return Ok(stockDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateStock stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();

            await _stockRepository.AddAsync(stockModel);
               

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel); 
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,  [FromBody] UpdateStock stockDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id,  stockDto.ToStockFromUpdateDTO());
            if(stockModel == null) return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {

            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null) return NotFound();
            return NoContent();
        }
    }

}
