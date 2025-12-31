namespace SampleApi.Models.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
    }
}
