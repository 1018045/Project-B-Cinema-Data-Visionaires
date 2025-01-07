using System.Text.Json.Serialization;

public class ReservationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("showingId")]
    public int ShowingId { get; set; }

    [JsonPropertyName("seats")]
    public string Seats { get; set; }

    [JsonPropertyName("paymentComplete")]
    public bool PaymentComplete { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("selectedExtras")]
    public List<ExtraModel> SelectedExtras { get; set; }

    [JsonPropertyName("billId")]
    public int BillId { get; set; }

    public ReservationModel(int id, int userId, int showingId, string seats, bool paymentComplete, double price, List<ExtraModel> selectedExtras)
    {
        Id = id;
        UserId = userId;
        ShowingId = showingId;
        Seats = seats;
        PaymentComplete = paymentComplete;
        Price = price;
        SelectedExtras = selectedExtras;
        BillId = -1; // Default value, will be set after bill creation
    }

    public void SetBillId(int billId)
    {
        BillId = billId;
    }
}
