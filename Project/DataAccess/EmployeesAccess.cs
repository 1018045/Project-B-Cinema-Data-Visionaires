using System.Text.Json;

static class EmployeesAccess
{
     static string path = Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/Employees.json"));

    public static List<EmployeeModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<EmployeeModel>>(json);
    }


    public static void WriteAll(List<EmployeeModel> reservations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(reservations.OrderBy(r => r.EmployeeID).ToList(), options);
        File.WriteAllText(path, json);
    }
}