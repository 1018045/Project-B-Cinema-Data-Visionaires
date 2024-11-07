using System.Text.Json.Serialization;

public class AdminModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    public AdminModel(int id, string emailAddress, string password)
    {
        Id = id;
        EmailAddress = emailAddress;
        Password = password;
    }
}