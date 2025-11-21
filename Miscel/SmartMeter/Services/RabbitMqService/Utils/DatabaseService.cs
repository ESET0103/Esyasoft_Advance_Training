using Microsoft.EntityFrameworkCore;
using Npgsql;
using SmartMeter.Data;
//using SmartMeter.Services.RabbitMqService.Model;
using SmartMeter.Models;

namespace SmartMeter.Services.RabbitMqService.Utils
{

    
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;
        //private readonly SmartMeterDbContext _context;
        private readonly IDbContextFactory<SmartMeterDbContext> _dbContextFactory;

        public DatabaseService(string connectionString, IDbContextFactory<SmartMeterDbContext> dbContextFactory)
        {
            _connectionString = connectionString;
            _dbContextFactory = dbContextFactory;
        }

        public async Task InsertMeterReadingAsync(Meterreading request)
        {
            //try
            //{
            //    await using var conn = new NpgsqlConnection(_connectionString);
            //    await conn.OpenAsync();

            //    string query = @"
            //    INSERT INTO meterreading (meterid, meterreadingdate, energyconsumed, voltage, current)
            //    VALUES (@meterid, @meterreadingdate, @energyconsumed, @voltage, @current);";

            //    await using var cmd = new NpgsqlCommand(query, conn);
            //    cmd.Parameters.AddWithValue("@meterid", data.meterid);
            //    cmd.Parameters.AddWithValue("@meterreadingdate", DateTime.Parse(data.meterreadingdate));
            //    cmd.Parameters.AddWithValue("@energyconsumed", data.energyconsumed);
            //    cmd.Parameters.AddWithValue("@voltage", data.voltage);
            //    cmd.Parameters.AddWithValue("@current", data.current);

            //    await cmd.ExecuteNonQueryAsync();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"[DB Error] {ex.Message}");
            //    throw;
            //}

            try
            {
               using (var _context = _dbContextFactory.CreateDbContext())
               {
                    var meterReading = new Meterreading
                    {
                        Meterid = request.Meterid,
                        Meterreadingdate = request.Meterreadingdate,
                        Energyconsumed = request.Energyconsumed,
                        Voltage = request.Voltage,
                        Current = request.Current
                    };

                await _context.Meterreadings.AddAsync(meterReading);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[DB Info] Meter reading inserted successfully for Meter ID: {request.Meterid}");
               }             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB Error] Failed to insert meter reading: {ex.Message}");
                throw;
            }

        }
    }
}
