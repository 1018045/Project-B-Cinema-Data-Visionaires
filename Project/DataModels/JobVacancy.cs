using System.Text.Json.Serialization;

public class JobVacancy
{
    [JsonPropertyName("id")]
    public int VacancyId { get; set; }

    [JsonPropertyName("jobTitle")]
    public string JobTitle { get; set; }

    [JsonPropertyName("jobDescription")]
    public string JobDescription { get; set; }

    [JsonPropertyName("datePosted")]
    public DateTime DatePosted { get; set; }

    [JsonPropertyName("salary")]
    public decimal? Salary { get; set; }

    [JsonPropertyName("employmentType")]
    public string EmploymentType { get; set; }

    public JobVacancy(int vacancyId, string jobTitle, string jobDescription, 
        DateTime datePosted, decimal? salary, string employmentType)
    {
        VacancyId = vacancyId;
        JobTitle = jobTitle;
      
        JobDescription = jobDescription;
        DatePosted = datePosted;
        Salary = salary;
        EmploymentType = employmentType;
    }
} 