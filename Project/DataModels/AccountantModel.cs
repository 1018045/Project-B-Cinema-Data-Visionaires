using System.Text.Json.Serialization;

public class AccountantModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("movieTitle")]
    public string MovieTitle { get; set; }

    [JsonPropertyName("ticketsSold")]
    public int TicketsSold { get; set; }

    [JsonPropertyName("revenue")]
    public decimal Revenue { get; set; }

    [JsonPropertyName("room")]
    public int Room { get; set; }

    public AccountantModel(int id, string date, string movieTitle, int ticketsSold, decimal revenue, int room)
    {
        Id = id;
        Date = date;
        MovieTitle = movieTitle;
        TicketsSold = ticketsSold;
        Revenue = revenue;
        Room = room;
    }
} 