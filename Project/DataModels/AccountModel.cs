using System.Text.Json.Serialization;


public abstract class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("totalSpent")]
    public double TotalSpent { get; set; }


    public AccountModel(int id, string role, string emailAddress, string password)
    {   
        Id = id;
        Role = role;
        EmailAddress = emailAddress;
        Password = password;
        TotalSpent = 0.0;
    }
}
