using System.Globalization;
using System.Runtime.InteropServices;
static class LoginMenu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login as User");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 to login as Admin");
        Console.WriteLine("Enter 6 to login as Accountant");
        Console.WriteLine("Enter 7 to exit program");

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
                AdminLogin.Start();
                break;
            case "6":
                AccountantMenu.Start();
                break;
            case "7":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }
}