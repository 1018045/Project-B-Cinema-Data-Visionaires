using System.Text.Json.Serialization;


public class MovieModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("minimumAge")]
    public int MinimumAge { get; set; }


    public MovieModel(int id, string title, int duration, int minimumAge)
    {
        Id = id;
        Title = title;
        Duration = duration;
        MinimumAge = minimumAge;
    }

}