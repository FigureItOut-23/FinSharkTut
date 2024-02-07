using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
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

        /*
            Explanation of the following:
            IActionResult is an interface in ASP.NET Core that represents the result of an action method. It's designed to provide flexibility in what an action method can return. 
            public IActionResult GetAll(): This is a public method named GetAll that returns an IActionResult. 
            IActionResult represents the result of an action method in ASP.NET Core, which can include various types of results such as views, JSON data, or HTTP status codes.
            
            var stocks = _context.Stocks.ToList();: Inside the method, it retrieves all the records from the Stocks table or entity set in the _context object. 
            Here, _context refers to an instance of a database context class. The Stocks property presumably represents a DbSet or collection of Stock entities defined within the context.

            .ToList(): This method is used to materialize the query and retrieve the data from the database as a list. 
            It executes the query and returns all the records from the Stocks table as a list of Stock objects.
        */
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /*
                In summary, this code snippet retrieves Stock entities from a database context, converts them into StockDto objects using the ToStockDto() extension method, 
                and stores the resulting sequence of StockDto objects in the variable stocks
            */
            var stocks =  await _stockRepository.GetAllAsync();

            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks); //This line returns an HTTP 200 OK response along with the stocks data
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById([FromRoute] int id)
        {
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
        [Route("{id}")]
        
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
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
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
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