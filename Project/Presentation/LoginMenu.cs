using System.Globalization;
using System.Runtime.InteropServices;
static class LoginMenu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 to exit program");
        Console.WriteLine("Enter 1 to login as User");
        Console.WriteLine("Enter 2 to login as Admin");
        Console.WriteLine("Enter 3 to create an account");
        Console.WriteLine("Enter 4 to login as Accountant");
        Console.WriteLine("Enter 5 to exit program");

        string input = Console.ReadLine();
        switch (input)
        {
            UserLogin.Start();
        }
        else if( input == "2")
        {
            AccountCreation.ChooseAccount();
        }
        else if( input == "3")
        {
            Showings.ShowUpcoming();
            Start();
        }
        else if( input == "4")
        {
            Showings.ShowUpcomingOnDate();
            Start();
        }
        else if (input == "5")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
            case "1":
                UserLogin.Start();
                break;
            case "2":
                AdminLogin.Start();
                break;
            case "3":
                AccountCreation.ChooseAccount();
                break;
            case "4":
                AccountantMenu.Start();
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