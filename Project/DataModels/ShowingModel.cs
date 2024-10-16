using System.Text.Json.Serialization;


public class ShowingModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("room")]
    public int Room { get; set; }

    [JsonPropertyName("minimumAge")]
    public int MinimumAge { get; set; }

    public ShowingModel(int id, string title, string date, string time, int room, int minimumAge)
    {
        Id = id;
        Title = title;
        Date = date;
        Time = time;
        Room = room;
        MinimumAge = minimumAge;
    }

}




