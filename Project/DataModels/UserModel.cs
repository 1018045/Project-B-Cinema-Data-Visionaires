using System.Text.Json.Serialization;


public class UserModel : AccountModel
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("birthDate")]
    public DateTime BirthDate { get; set; }

    public UserModel(int id, string emailAddress, string password, string fullName, DateTime birthDate) : base(id, "user", emailAddress, password)
    {   
        FullName = fullName;
        BirthDate = birthDate;
    }

}
