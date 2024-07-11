using Finshark.Data;
using Finshark.Dtos.stock;
using Finshark.Helpers;
using Finshark.Interfaces;
using Finshark.Models;
using Microsoft.EntityFrameworkCore;

namespace Finshark.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null) 
            {
                return null;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stock = _context.Stocks.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName)) 
            {
                stock = stock.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stock = stock.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy)) 
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) 
                {
                    stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol);
                }
            }
            var skipNumber = (query.PageNumber -1)* query.PageSize;

            return await stock.Skip(skipNumber).Take(query.PageNumber).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(c => c.Symbol == symbol);
        }

        public Task<bool> stockExists(int id)
        {
          return _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null) 
            {
                return null; 
            }
            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.MarketCap = stockDto.MarketCap;
            existingStock.Industry = stockDto.Industry;

            await _context.SaveChangesAsync();
            return existingStock;
        }

    }
} 
