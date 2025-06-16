namespace Server.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BuildId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
}