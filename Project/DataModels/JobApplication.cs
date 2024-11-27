using System.Text.Json.Serialization;

public class JobApplication
{
    [JsonPropertyName("id")]
    public int ApplicationId { get; set; }

    [JsonPropertyName("vacancyId")]
    public int VacancyId { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("motivation")]
    public string Motivation { get; set; }

    [JsonPropertyName("dateApplied")]
    public DateTime DateApplied { get; set; }

    public JobApplication(int applicationId, int vacancyId, string email, string motivation, DateTime dateApplied)
    {
        ApplicationId = applicationId;
        VacancyId = vacancyId;
        Email = email;
        Motivation = motivation;
        DateApplied = dateApplied;
    }
} 