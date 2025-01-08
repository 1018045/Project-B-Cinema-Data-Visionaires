using System.Text.Json;
using System.Text.Json.Nodes;
using Project.DataModels;

static class AccountsAccess
{
    static string path = Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/accounts.json"));


    public static List<AccountModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        JsonNode preAccountList = JsonNode.Parse(json);
        List<AccountModel> output = new();
        foreach (JsonObject acc in preAccountList.AsArray())
        {
            string role = acc["role"].ToString();
            switch (role)
            {
                case "user":
                    output.Add(acc.Deserialize<UserModel>());
                    break;
                case "admin":
                    output.Add(acc.Deserialize<AdminModel>());
                    break;
                case "accountant":
                    output.Add(acc.Deserialize<AccountantModel>());
                    break;
                case "staff":
                    output.Add(acc.Deserialize<StaffModel>());
                    break;
                default:
                    System.Console.WriteLine($@"Error: Please check user {acc["id"]} in accounts database");
                    break;
            }
        }
        return output;
    }

    public static void WriteAll(List<AccountModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}