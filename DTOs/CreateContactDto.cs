namespace SampleApi.Models.DTOs
{
    public class CreateContactDto
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
