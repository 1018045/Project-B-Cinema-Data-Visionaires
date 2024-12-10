using System.Text.Json;

static class EmployeesAccess
{
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/Employees.json"));

    public static List<EmployeeModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<EmployeeModel>>(json);
    }

    public static void WriteAll(List<EmployeeModel> employees)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(employees, options);
        File.WriteAllText(path, json);
    }
}