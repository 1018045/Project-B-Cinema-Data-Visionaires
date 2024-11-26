using System.Text.Json;

static class JobApplicationAccess
{
    static string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"DataSources/applications.json"));

    public static List<JobApplication> LoadAll()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
            return new List<JobApplication>();
        }
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<JobApplication>>(json) ?? new List<JobApplication>();
    }

    public static void WriteAll(List<JobApplication> applications)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(applications.OrderBy(a => a.ApplicationId).ToList(), options);
        File.WriteAllText(path, json);
    }
} 