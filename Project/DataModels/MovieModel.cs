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

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("actors")]
    public List<string> Actors { get; set; }

    [JsonPropertyName("director")]
    public string Director { get; set; }


    public MovieModel(int id, string title, int duration, int minimumAge, string summary, List<string> actors, string director)
    {
        Id = id;
        Title = title;
        Duration = duration;
        MinimumAge = minimumAge;
        Summary = summary;
        Actors = actors;
        Director = director;
    }

}