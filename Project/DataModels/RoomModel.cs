using System.Text.Json.Serialization;

public class RoomModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("layout")]
    public List<string> Layout { get; set; }
}


