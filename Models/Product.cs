using System.Text.Json.Serialization;

namespace SampleApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
    }
} 
