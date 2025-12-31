using System.Text.Json.Serialization;

namespace SampleApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
    }
}
