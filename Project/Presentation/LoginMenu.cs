using System.Globalization;
using System.Runtime.InteropServices;

static class LoginMenu
{
    static public void Start()
    {
        Console.Clear();
        ShowMenu();
        ProcessUserInput();
    }

    private static void ShowMenu()
    {
        Console.WriteLine("=== Cinema Menu ===");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Create Account");
        Console.WriteLine("3. View Upcoming Movies");
        Console.WriteLine("4. Search Movies by Date");
        Console.WriteLine("5. View Job Vacancies");
        Console.WriteLine("6. Admin Menu");
        Console.WriteLine("7. Exit Program");
        Console.WriteLine("==================");
    }

    private static void ProcessUserInput()
    {
        string input = Console.ReadLine();
        
        switch (input)
        {
            case "1":
                UserLogin.Start();
                break;
            
            case "2":
                AccountCreation.ChooseAccount();
                break;
            
            case "3":
                Showings.ShowUpcoming();
                Start();
                break;
            
            case "4":
                Showings.ShowUpcomingOnDate();
                Start();
                break;
            
            case "5":
                ApplyForJob.ShowJobMenu();
                break;
            
            case "6":
                AdminMenu.Start();
                break;
            
            case "7":
                Environment.Exit(0);
                break;
            
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                Thread.Sleep(2000);
                Start();
                break;
        }
    }
}


