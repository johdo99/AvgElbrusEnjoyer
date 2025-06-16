using System.Collections.Generic;
namespace Server.Api;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public List<int> ComponentIds { get; set; }
}