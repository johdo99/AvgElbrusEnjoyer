using System.Text.Json.Serialization;

namespace Client.Models;

public class ComponentDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Type { get; set; }

    public decimal Price { get; set; }
    public int Stock { get; set; }

    [JsonIgnore]
    public string TypeAsString => ((ComponentType)Type).ToString();
}

public enum ComponentType
{
    CPU,
    GPU,
    RAM,
    Storage,
    Motherboard,
    PSU,
    Cooling,
    Case
}