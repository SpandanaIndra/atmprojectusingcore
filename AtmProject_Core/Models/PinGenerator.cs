namespace AtmProject_Core.Models
{
    public static class PinGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateRandomPin()
        {
            int pin = _random.Next(1000, 10000); // Generates a random number between 1000 and 9999
            return pin.ToString("D4"); // Formats the pin as a 4-digit string with leading zeros
        }
    }
}
