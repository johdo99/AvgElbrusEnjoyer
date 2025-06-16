namespace Server.Models;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ComponentType Type { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}