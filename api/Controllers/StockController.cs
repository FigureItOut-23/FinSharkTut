using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    /*
        Explanation of the following:
        [Route("api/stock")]: This is an attribute in ASP.NET Core used for routing HTTP requests to specific controller actions. 
        In this case, it specifies that the routes for actions within the controller are prefixed with api/stock. 
        So, for example, an action method named GetAll in this controller would be accessible via a URL like /api/stock/GetAll.
    */
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        
        /*
            Explanation of the following Method:
            In this case, StockController relies on an instance of ApplicationDBContext, which presumably represents a database context used for interacting with a database. 
            By passing an instance of ApplicationDBContext to the constructor, the StockController class can use it to perform database operations within its methods.
        */
        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

      
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stocks =  await _stockRepository.GetAllAsync(query);

            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks); //This line returns an HTTP 200 OK response along with the stocks data
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //before DI: var stock = await _context.Stocks.FindAsync(id);
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }   
            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        //you need the FromBody because the data is being sent in the form of a JSON. Not passing data through URL, passing it in body of HTTP
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            //  before:  await _context.Stocks.AddAsync(stockModel);
            //    await _context.SaveChangesAsync();
            await _stockRepository.CreateAsync(stockModel);
            /*
                CreatedAtAction is going to pass in the new object into the GetById method and return in the form of a ToStockDto
                The first two arguments is basically "Create a URL that points to a method called GetById, and pass in the id of the newly created stock as a route parameter." 
                This URL will then be included in the response headers so the client knows where to find the newly created resource.
            */
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id:int}")]
        
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // When the retrieval happens, EF starts tracking the object

            //before DI: var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

            if (stockModel == null)
            {
                return NotFound();
            }


            // before DI: stockModel.Symbol = updateDto.Symbol;
            // stockModel.CompanyName = updateDto.CompanyName;
            // stockModel.Purchase = updateDto.Purchase;
            // stockModel.LastDiv = updateDto.LastDiv;
            // stockModel.Industry = updateDto.Industry;
            // stockModel.MarketCap = updateDto.MarketCap;

            // await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //before DI:var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            //before DI: _context.Stocks.Remove(stockModel);

            //Status 204
            return NoContent();

        }
    }
}