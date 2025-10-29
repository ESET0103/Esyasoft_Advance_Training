using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
namespace SmartMeter.Services.TariffServices
{
    public class TariffServices : ITariffServices
    {
        private readonly SmartMeterDbContext _context;
      

        public TariffServices (SmartMeterDbContext context)
        {
            _context = context;
         
        }

        public async Task<IEnumerable<Tariff>> GetAllTariffsAsync()
        {
            Console.WriteLine("Enters the function to get the list...");

            var listofTariff = await _context.Tariffs
                .Include(t => t.Todrules.Where(r => !r.Deleted))
                .ToListAsync();

            Console.WriteLine(listofTariff);
            return listofTariff;
        }

        public async Task<Tariff> GetTariffByIdAsync(int tariffId)
        {
            return await _context.Tariffs.Include(t => t.Todrules.Where(r => !r.Deleted))
                .FirstOrDefaultAsync(t => t.Tariffid == tariffId);
        }

        public async Task<IEnumerable<Todrule>> GetTodRulesByTariffAsync(int tariffId)
        {
            return await _context.Todrules
                .Where(r => r.Tariffid == tariffId && !r.Deleted)
                .ToListAsync();
        }


    }
}
