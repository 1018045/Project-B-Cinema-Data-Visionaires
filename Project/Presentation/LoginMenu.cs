using System.Globalization;
using System.Runtime.InteropServices;
static class LoginMenu
{
    static public void Start()
    {
        // Console.Clear();
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 to exit program");

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
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }
}