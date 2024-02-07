using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        // Overall, this code snippet provides a convenient way to convert Stock objects to StockDto objects using an extension method, which can be called directly on instances of the Stock class. 
        // This pattern is commonly used in C# to encapsulate conversion logic
        public static StockDto ToStockDto (this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Commments.Select(c=> c.ToCommentDto()).ToList()
            };
        }
        public static Stock ToStockFromCreateDto (this CreateStockRequestDto stockDto)
        {
            return new Stock{
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}