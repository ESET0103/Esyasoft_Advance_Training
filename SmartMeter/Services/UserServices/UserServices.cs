using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models.DTOs;

namespace SmartMeter.Services.UserServices
{
    public class UserServices : IUserServices
    {

        public readonly SmartMeterDbContext _context;
        public UserServices(SmartMeterDbContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<HistoricalConsumptionDto>> GetHistoricalConsumptionAsync(
        //    int orgUnitId, DateTime startDate, DateTime endDate)
        //{
        //    var sql = @"
        //        WITH RECURSIVE OrgHierarchy AS (
        //            SELECT OrgUnitId, Name, ParentId, Type
        //            FROM ""OrgUnit""
        //            WHERE OrgUnitId = {0}
        //            UNION ALL
        //            SELECT o.""OrgUnitId"", o.""Name"", o.""ParentId"", o.""Type""
        //            FROM ""OrgUnit"" o
        //            INNER JOIN OrgHierarchy oh ON o.""ParentId"" = oh.""OrgUnitId""
        //        )
        //        SELECT 
        //            oh.""OrgUnitId"",
        //            oh.""Name"" AS ""OrgUnitName"",
        //            COALESCE(SUM(b.""TotalUnitsConsumed""), 0) AS ""TotalUnits"",
        //            COALESCE(SUM(b.""TotalAmount""), 0) AS ""TotalAmount""
        //        FROM OrgHierarchy oh
        //        LEFT JOIN ""Consumer"" c ON c.""OrgUnitId"" = oh.""OrgUnitId""
        //        LEFT JOIN ""Billing"" b
        //            ON b.""ConsumerId"" = c.""ConsumerId""
        //            AND b.""BillingPeriodStart"" >= {1}
        //            AND b.""BillingPeriodEnd"" <= {2}
        //        GROUP BY oh.""OrgUnitId"", oh.""Name""
        //        ORDER BY oh.""OrgUnitId"";
        //    "
        //    ;

        //    return await _context.Set<HistoricalConsumptionDto>()
        //        .FromSqlRaw(sql, orgUnitId, startDate, endDate)
        //        .ToListAsync();
        //}
        public async Task<HistoricalConsumptionDto> GetHistoricalConsumptionAsync(int orgUnitId, DateTime startDate, DateTime endDate)
        {
            // Ensure dates are UTC to prevent PostgreSQL timestamp issues
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            // ✅ Fetch OrgUnit details first
            var orgUnit = await _context.Orgunits
                .FirstOrDefaultAsync(o => o.Orgunitid == orgUnitId);

            if (orgUnit == null)
                return null; // OrgUnit not found

            // ✅ Fetch total energy consumed via join from OrgUnit → Consumer → Meter → MeterReading
            var totalEnergyConsumed = await (
                from ou in _context.Orgunits
                join c in _context.Consumers on ou.Orgunitid equals c.Orgunitid
                join m in _context.Meters on c.Consumerid equals m.Consumerid
                join r in _context.Meterreadings on m.Meterserialno equals r.Meterid
                where ou.Orgunitid == orgUnitId
                      && r.Meterreadingdate >= startDate
                      && r.Meterreadingdate <= endDate
                select r.Energyconsumed
            ).SumAsync();

            // ✅ Return the DTO with results
            return new HistoricalConsumptionDto
            {
                OrgUnitName = orgUnit.Name,
                StartDate = startDate,
                EndDate = endDate,
                TotalEnergyConsumed = totalEnergyConsumed
            };
        }
    }
}
