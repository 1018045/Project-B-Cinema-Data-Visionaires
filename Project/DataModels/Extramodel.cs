using System.Text.Json.Serialization;

public class ExtraModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("is_mandatory")]
    public bool IsMandatory { get; set; }


    public ExtraModel(string name, decimal price, bool isMandatory)
    {
        Name = name;
        Price = price;
        IsMandatory = isMandatory;
    }
}
