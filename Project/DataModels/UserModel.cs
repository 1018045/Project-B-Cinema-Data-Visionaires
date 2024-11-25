using System.Text.Json.Serialization;


public class UserModel : AccountModel
{
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    public UserModel(int id, string emailAddress, string password, string fullName, int age) : base(id, "user", emailAddress, password)
    {   
        FullName = fullName;
        Age = age;        
    }

}
