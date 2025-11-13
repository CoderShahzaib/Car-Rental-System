namespace CarRentalSystem.Core.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
