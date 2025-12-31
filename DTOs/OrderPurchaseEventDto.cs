public class OrderPurchaseEventDto
{
    public int OrderId { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
}
