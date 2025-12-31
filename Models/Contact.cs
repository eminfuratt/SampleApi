namespace SampleApi.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public int UserId { get; set; }

        public User? User { get; set; }  // Navigation property
    }
}
