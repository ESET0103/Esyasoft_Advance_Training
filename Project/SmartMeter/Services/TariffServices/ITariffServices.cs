using SmartMeter.Models;

namespace SmartMeter.Services.TariffServices
{
    public interface ITariffServices
    {
        Task<IEnumerable<Tariff>> GetAllTariffsAsync();
        Task<Tariff> GetTariffByIdAsync(int tariffId);
        Task<IEnumerable<Todrule>> GetTodRulesByTariffAsync(int tariffId);


    }
}
