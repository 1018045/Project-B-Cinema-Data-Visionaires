using System.Text.Json.Serialization;

public class Item
{
    [JsonPropertyName("name")]
    public string Name {get;set;}

    [JsonPropertyName("price")]
    public double Price {get;set;}

    


    public Item(string name, double price)
    {
        Name = name; 
        Price = price; 
        
    }
}