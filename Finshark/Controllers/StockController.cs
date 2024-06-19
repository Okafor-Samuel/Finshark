using Finshark.Data;
using Finshark.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Finshark.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase 
    {

        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
               
        }
        [HttpGet]
        public IActionResult GetAll()
        {
          var stocks = _context.Stocks.ToList()
                .Select(s => s.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) 
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null) 
            {
                return NotFound();            
            }
            return Ok(stock.ToStockDto());
        }
    }
}
