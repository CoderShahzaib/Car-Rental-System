namespace CarRentalSystem.Core.DTOs
{
    public class CarDTO
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public decimal DailyRate { get; set; }

        public string Color { get; set; }

        public string carImage { get; set; }
    }
}
