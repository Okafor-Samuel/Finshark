using Finshark.Data;
using Finshark.Dtos.stock;
using Finshark.Mappers.stock;
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
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) 
        {
            var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stockModel == null) 
            {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.MarketCap = updateDto.MarketCap;
            stockModel.Industry = updateDto.Industry;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());

        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id) 
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if (stockModel == null) 
            {  
                return NotFound(); 
            }
            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();
            return NoContent();

        }
        
    }
}
