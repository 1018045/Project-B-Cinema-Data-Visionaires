using System.Text.Json.Serialization;


public class AccountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

   
    public int Age {get;set;}

    

    public AccountModel(int id, string emailAddress, string password, string fullName, int age)
    {   
        Id++;
        EmailAddress = emailAddress;
        Password = password;
        FullName = fullName;
        Age = age;
        
    }

}
