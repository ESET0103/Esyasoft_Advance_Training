using rabbitmqPractice.DTOs;

namespace rabbitmqPractice.Services
{
    public class DataGenerator
    {
        private readonly Random _random = new Random();

        public async Task StartGeneratingAsync(Func<MeterReadingDto, Task> onGenerated, CancellationToken token)
        {
            Console.WriteLine("🔁 Starting random meter data generation every 1 minute...");

            while (!token.IsCancellationRequested)
            {
                var meterData = GenerateRandomReading();

                // Pass generated data to the callback (e.g., publisher)
                await onGenerated(meterData);

                // Wait 1 minute before generating the next reading
                await Task.Delay(TimeSpan.FromMinutes(1), token);
            }
        }
        private MeterReadingDto GenerateRandomReading()
        {
            return new MeterReadingDto
            {
                Meterid = "MTR" + _random.Next(1, 5), // 4 meters: MTR1–MTR4
                Meterreadingdate = DateTime.UtcNow,
                Energyconsumed = Math.Round((decimal)(_random.NextDouble() * 100), 2),
                Voltage = Math.Round((decimal)(200 + _random.NextDouble() * 20), 2),
                Current = Math.Round((decimal)(_random.NextDouble() * 10), 2),
                RoutingKey = "meter.reading"
            };
        }

    }
}
