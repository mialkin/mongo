namespace Mongo.Data.Models;

public record Order(int CartId, List<Line> Lines)
{
    public long OrderId { get; set; }
}
