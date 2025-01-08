using System.Text.Json.Serialization;

public class CinemaModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("postal_code")]
    public string PostalCode { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }


   public CinemaModel(int id, string name, string city, string address, string postalCode, string phoneNumber)
    {
        Id = id;
        Name = name;
        City = city;
        Address = address;
        PostalCode = postalCode;      
        PhoneNumber = phoneNumber;  
    }
}
