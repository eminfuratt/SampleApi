namespace SampleApi.Models.DTOs
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
