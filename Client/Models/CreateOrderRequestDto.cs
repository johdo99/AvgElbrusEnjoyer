using System.Collections.Generic;

namespace Client.Models;

public class CreateOrderRequestDto
{
    public int UserId { get; set; }
    public List<int> ComponentIds { get; set; }
}