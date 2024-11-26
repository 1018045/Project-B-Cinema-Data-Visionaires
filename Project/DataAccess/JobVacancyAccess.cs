using System.Text.Json;

static class JobVacancyAccess
{
    static string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"DataSources/vacancies.json"));

    public static List<JobVacancy> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<JobVacancy>>(json);
    }

    public static void WriteAll(List<JobVacancy> vacancies)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(vacancies.OrderBy(v => v.VacancyId).ToList(), options);
        File.WriteAllText(path, json);
    }
} 