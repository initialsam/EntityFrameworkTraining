using System.Text.Json.Serialization;

namespace Core6;

public class Product
{
   
    public ProductId Id { get; set; }

    public string Name { get; set; }

    public int Price { get; set; }

    public Guid? Code { get; set; }
}
