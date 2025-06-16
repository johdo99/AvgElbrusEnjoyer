namespace Server.Models;


public class ComputerBuild
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Category { get; set; }
    public List<Component> Components { get; set; } = new List<Component>();

    public decimal TotalPrice => Components.Sum(c => c.Price);
}